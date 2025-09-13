```sql
WITH
att AS (
  SELECT
    ca.attack_id,
    ca.creature_id,
    ca.date::date                 AS dt,
    ca.location_id,
    ca.outcome,
    ca.casualties::numeric        AS own_casualties,
    ca.enemy_casualties::numeric  AS enemy_casualties,
    ca.military_response_time_minutes::int AS response_min,
    EXTRACT(YEAR    FROM ca.date)::int    AS yr,
    EXTRACT(QUARTER FROM ca.date)::int    AS qr,
    EXTRACT(MONTH   FROM ca.date)::int    AS mn,
    CASE
      WHEN EXTRACT(MONTH FROM ca.date) IN (12,1,2) THEN 'winter'
      WHEN EXTRACT(MONTH FROM ca.date) IN (3,4,5)  THEN 'spring'
      WHEN EXTRACT(MONTH FROM ca.date) IN (6,7,8)  THEN 'summer'
      ELSE 'autumn'
    END AS season,
    CASE WHEN ca.outcome ILIKE 'victory'
           OR ca.outcome ILIKE 'defended'
         THEN 1 ELSE 0 END AS success_flag,
    CASE WHEN ca.outcome ILIKE 'breach'
           OR ca.outcome ILIKE 'defeat'
         THEN 1 ELSE 0 END AS breach_flag
  FROM creature_attacks ca
),
overall AS (
  SELECT
    COUNT(*)                                   AS total_recorded_attacks,
    COUNT(DISTINCT creature_id)                AS unique_attackers,
    ROUND(100.0 * SUM(success_flag)::numeric / NULLIF(COUNT(*),0), 2) AS overall_defense_success_rate
  FROM att
),
last_sight AS (
  SELECT
    cs.creature_id,
    MAX(cs.date)::date AS last_sighting_date
  FROM creature_sightings cs
  GROUP BY cs.creature_id
),
territory AS (
  SELECT
    ct.creature_id,
    MIN(ct.distance_to_fortress)::numeric AS territory_proximity
  FROM creature_territories ct
  GROUP BY ct.creature_id
),
cre_base AS (
  SELECT
    c.creature_id,
    c.type AS creature_type,
    c.threat_level,
    c.estimated_population
  FROM creatures c
  WHERE c.active = TRUE
),
threats AS (
  SELECT
    cb.creature_type,
    MAX(cb.threat_level)                               AS threat_level,
    MAX(ls.last_sighting_date)                         AS last_sighting_date,
    MIN(te.territory_proximity)                        AS territory_proximity,
    COALESCE(SUM(cb.estimated_population),0)           AS estimated_numbers,
    json_agg(cb.creature_id ORDER BY cb.creature_id)   AS creature_ids
  FROM cre_base cb
  LEFT JOIN last_sight ls ON ls.creature_id = cb.creature_id
  LEFT JOIN territory te  ON te.creature_id = cb.creature_id
  GROUP BY cb.creature_type
),

current_threat AS (
  SELECT
    CASE
      WHEN COALESCE(MAX(threat_level),0) >= 5 THEN 'Severe'
      WHEN COALESCE(MAX(threat_level),0) >= 3 THEN 'Moderate'
      WHEN COALESCE(MAX(threat_level),0) >  0 THEN 'Low'
      ELSE 'None'
    END AS current_threat_level
  FROM threats
),

zone_stats AS (
  SELECT
    l.zone_id,
    l.name AS zone_name,
    l.fortification_level,
    COUNT(a.attack_id)                                    AS attacks_here,
    SUM(a.breach_flag)                                    AS historical_breaches,
    ROUND(AVG(NULLIF(a.response_min,0))::numeric, 0)      AS military_response_time
  FROM locations l
  LEFT JOIN att a ON a.location_id = l.location_id
  GROUP BY l.zone_id, l.name, l.fortification_level
),
zone_norm AS (
  SELECT
    MAX(historical_breaches) AS max_breaches,
    MIN(fortification_level) AS min_fort_lvl,
    MAX(fortification_level) AS max_fort_lvl,
    MAX(military_response_time) AS max_resp
  FROM zone_stats
),
zone_scored AS (
  SELECT
    z.*,
    ROUND(
      (
        (COALESCE(z.historical_breaches,0)::numeric / NULLIF(GREATEST(n.max_breaches,1),0)) * 0.5 +
        (COALESCE(z.military_response_time,0)::numeric / NULLIF(GREATEST(n.max_resp,1),0)) * 0.3 +
        (1.0 - COALESCE(z.fortification_level,0)::numeric / NULLIF(GREATEST(n.max_fort_lvl,1),0)) * 0.2
      )
    , 2) AS vulnerability_score
  FROM zone_stats z CROSS JOIN zone_norm n
),

def_eff AS (
  SELECT
    l.zone_type                               AS defense_type,
    ROUND(100.0 * AVG(a.success_flag)::numeric, 2) AS effectiveness_rate,
    ROUND(AVG(COALESCE(a.enemy_casualties,0)), 1)  AS avg_enemy_casualties,
    json_agg(DISTINCT l.location_id ORDER BY l.location_id) AS structure_ids
  FROM att a
  JOIN locations l ON l.location_id = a.location_id
  GROUP BY l.zone_type
),

active_members AS (
  SELECT
    sm.squad_id,
    COUNT(DISTINCT sm.dwarf_id) FILTER (WHERE sm.exit_date IS NULL) AS active_members
  FROM squad_members sm
  GROUP BY sm.squad_id
),
combat_levels AS (
  SELECT
    wc.squad_id,
    AVG(ls.level::numeric) AS avg_combat_skill
  FROM squad_members wc
  JOIN LATERAL (
    SELECT ds.level
    FROM dwarf_skills ds
    JOIN skills s ON s.skill_id = ds.skill_id
    WHERE ds.dwarf_id = wc.dwarf_id
      AND (s.category ILIKE 'combat' OR s.skill_type ILIKE 'combat')
    ORDER BY ds.date DESC
    LIMIT 1
  ) ls ON TRUE
  GROUP BY wc.squad_id
),
combat_effect AS (
  SELECT
    sb.squad_id,
    ROUND(AVG(CASE WHEN sb.outcome ILIKE 'victory' THEN 1.0 ELSE 0.0 END)::numeric, 2) AS combat_effectiveness
  FROM squad_battles sb
  GROUP BY sb.squad_id
),
readiness_raw AS (
  SELECT
    s.squad_id,
    s.name AS squad_name,
    COALESCE(am.active_members,0)                AS active_members,
    COALESCE(cl.avg_combat_skill,0)              AS avg_combat_skill,
    COALESCE(ce.combat_effectiveness,0)          AS combat_effectiveness,
    (SELECT COUNT(*) FROM squad_training st
      WHERE st.squad_id = s.squad_id
        AND st.date >= CURRENT_DATE - INTERVAL '60 days')::numeric AS recent_train_cnt,
    (
      SELECT
        SUM(e.quality::numeric * COALESCE(se.quantity,1)) / NULLIF(SUM(COALESCE(se.quantity,1)),0)
      FROM squad_equipment se
      JOIN equipment e ON e.equipment_id = se.equipment_id
      WHERE se.squad_id = s.squad_id
    )::numeric AS equipment_quality
  FROM military_squads s
  LEFT JOIN active_members am ON am.squad_id = s.squad_id
  LEFT JOIN combat_levels  cl ON cl.squad_id = s.squad_id
  LEFT JOIN combat_effect  ce ON ce.squad_id = s.squad_id
),
readiness_norm AS (
  SELECT
    MAX(active_members)        AS max_m,
    MAX(avg_combat_skill)      AS max_sk,
    MAX(combat_effectiveness)  AS max_ce,
    MAX(recent_train_cnt)      AS max_tr,
    MAX(equipment_quality)     AS max_eq
  FROM readiness_raw
),
readiness AS (
  SELECT
    r.squad_id,
    r.squad_name,
    ROUND((
      0.30 * (COALESCE(r.avg_combat_skill,0)     / NULLIF(n.max_sk,0)) +
      0.30 * (COALESCE(r.combat_effectiveness,0) / NULLIF(n.max_ce,0)) +
      0.20 * (COALESCE(r.recent_train_cnt,0)     / NULLIF(n.max_tr,0)) +
      0.20 * (COALESCE(r.equipment_quality,0)    / NULLIF(n.max_eq,0))
    )::numeric, 2) AS readiness_score,
    COALESCE(r.active_members,0)   AS active_members,
    ROUND(COALESCE(r.avg_combat_skill,0), 2)       AS avg_combat_skill,
    ROUND(COALESCE(r.combat_effectiveness,0), 2)   AS combat_effectiveness
  FROM readiness_raw r CROSS JOIN readiness_norm n
),
squad_zone_cover AS (
  SELECT
    st.squad_id,
    st.location_id AS zone_id,
    COALESCE(
      PERCENTILE_CONT(0.5) WITHIN GROUP (ORDER BY a.response_min)
      FILTER (WHERE a.response_min IS NOT NULL), 0
    )::int AS response_time
  FROM squad_training st
  LEFT JOIN att a ON a.location_id = st.location_id
  GROUP BY st.squad_id, st.location_id
),

yearly AS (
  SELECT
    a.yr AS year,
    COUNT(*) AS total_attacks,
    SUM(a.success_flag) AS success_cnt,
    SUM(COALESCE(a.own_casualties,0))::numeric AS casualties
  FROM att a
  GROUP BY a.yr
),
yearly_rates AS (
  SELECT
    y.year,
    ROUND(100.0 * y.success_cnt::numeric / NULLIF(y.total_attacks,0), 2) AS defense_success_rate,
    y.total_attacks,
    y.casualties,
    ROUND( (COALESCE(y.success_cnt::numeric,0) / NULLIF(y.total_attacks,0))
         - (COALESCE(LAG(y.success_cnt) OVER (ORDER BY y.year),0)::numeric
            / NULLIF(LAG(y.total_attacks) OVER (ORDER BY y.year),0)), 4) * 100
      AS year_over_year_improvement
  FROM yearly y
),

threat_assessment_json AS (
  SELECT json_build_object(
           'current_threat_level', (SELECT current_threat_level FROM current_threat),
           'active_threats', COALESCE(json_agg(
             json_build_object(
               'creature_type',      t.creature_type,
               'threat_level',       t.threat_level,
               'last_sighting_date', t.last_sighting_date,
               'territory_proximity', t.territory_proximity,
               'estimated_numbers',  t.estimated_numbers,
               'creature_ids',       COALESCE(t.creature_ids, '[]'::json)
             )
             ORDER BY t.threat_level DESC, t.last_sighting_date DESC NULLS LAST
           ), '[]'::json)
         ) AS obj
  FROM threats t
),

vulnerability_analysis_json AS (
  SELECT COALESCE(json_agg(
           json_build_object(
             'zone_id',               zs.zone_id,
             'zone_name',             zs.zone_name,
             'vulnerability_score',   zs.vulnerability_score,
             'historical_breaches',   zs.historical_breaches,
             'fortification_level',   zs.fortification_level,
             'military_response_time', zs.military_response_time,
             'defense_coverage', json_build_object(
               -- используем location_id как proxy структуры
               'structure_ids', COALESCE((
                   SELECT json_agg(l.location_id ORDER BY l.location_id)
                   FROM locations l
                   WHERE l.zone_id = zs.zone_id
               ), '[]'::json),
               'squad_ids', COALESCE((
                   SELECT json_agg(DISTINCT st.squad_id ORDER BY st.squad_id)
                   FROM squad_training st
                   JOIN locations l2 ON l2.location_id = st.location_id
                   WHERE l2.zone_id = zs.zone_id
               ), '[]'::json)
             )
           )
           ORDER BY zs.vulnerability_score DESC
         ), '[]'::json) AS arr
  FROM zone_scored zs
),

defense_effectiveness_json AS (
  SELECT COALESCE(json_agg(
           json_build_object(
             'defense_type',         d.defense_type,
             'effectiveness_rate',   d.effectiveness_rate,
             'avg_enemy_casualties', d.avg_enemy_casualties,
             'structure_ids',        COALESCE(d.structure_ids, '[]'::json)
           )
           ORDER BY d.effectiveness_rate DESC
         ), '[]'::json) AS arr
  FROM def_eff d
),

military_readiness_json AS (
  SELECT COALESCE(json_agg(
           json_build_object(
             'squad_id',              s.squad_id,
             'squad_name',            s.squad_name,
             'readiness_score',       s.readiness_score,
             'active_members',        s.active_members,
             'avg_combat_skill',      s.avg_combat_skill,
             'combat_effectiveness',  s.combat_effectiveness,
             'response_coverage', COALESCE((
                SELECT json_agg(json_build_object(
                          'zone_id',      q.zone_id,
                          'response_time', q.response_time))
                FROM squad_zone_cover q
                WHERE q.squad_id = s.squad_id
             ), '[]'::json)
           )
           ORDER BY s.readiness_score DESC
         ), '[]'::json) AS arr
  FROM readiness s
),

security_evolution_json AS (
  SELECT COALESCE(json_agg(
           json_build_object(
             'year',                     y.year,
             'defense_success_rate',     y.defense_success_rate,
             'total_attacks',            y.total_attacks,
             'casualties',               y.casualties,
             'year_over_year_improvement', y.year_over_year_improvement
           )
           ORDER BY y.year
         ), '[]'::json) AS arr
  FROM yearly_rates y
)

SELECT json_build_object(
  'total_recorded_attacks',     (SELECT total_recorded_attacks FROM overall),
  'unique_attackers',           (SELECT unique_attackers       FROM overall),
  'overall_defense_success_rate', (SELECT overall_defense_success_rate FROM overall),
  'security_analysis', json_build_object(
    'threat_assessment',          (SELECT obj FROM threat_assessment_json),
    'vulnerability_analysis',     (SELECT arr FROM vulnerability_analysis_json),
    'defense_effectiveness',      (SELECT arr FROM defense_effectiveness_json),
    'military_readiness_assessment', (SELECT arr FROM military_readiness_json),
    'security_evolution',         (SELECT arr FROM security_evolution_json)
  )
) AS fortress_security_json;

```
