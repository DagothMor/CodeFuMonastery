```sql
WITH

militarySquads AS (
  SELECT
    s.squad_id,
    s.name AS squad_name,
    s.formation_type,
    d.name AS leader_name
  FROM military_squads s
  LEFT JOIN dwarves d ON d.dwarf_id = s.leader_id
),

membrs AS (
  SELECT
    sm.squad_id,
    COUNT(DISTINCT sm.dwarf_id) AS total_members_ever,
    COUNT(DISTINCT sm.dwarf_id) FILTER (WHERE sm.exit_date IS NULL) AS current_members,
    json_agg(DISTINCT sm.dwarf_id ORDER BY sm.dwarf_id) AS member_ids_all,
    json_agg(DISTINCT sm.dwarf_id) FILTER (WHERE sm.exit_date IS NULL) AS member_ids_current
  FROM squad_members sm
  GROUP BY sm.squad_id
),

squadBattles AS (
  SELECT
    sb.squad_id,
    COUNT(*) AS total_battles,
    COUNT(*) FILTER (WHERE sb.outcome = 'victory') AS victories,
    COALESCE(SUM(sb.casualties), 0)::numeric AS own_casualties,
    COALESCE(SUM(sb.enemy_casualties), 0)::numeric AS enemy_casualties,
    json_agg(DISTINCT sb.report_id ORDER BY sb.report_id) AS battle_report_ids
  FROM squad_battles sb
  GROUP BY sb.squad_id
),

squadTraining AS (
  SELECT
    st.squad_id,
    COUNT(*) AS total_training_sessions,
    AVG(st.effectiveness)::numeric AS avg_training_effectiveness,
    json_agg(DISTINCT st.schedule_id ORDER BY st.schedule_id) AS training_ids
  FROM squad_training st
  GROUP BY st.squad_id
),

squadEquipment AS (
  SELECT
    se.squad_id,
    SUM(e.quality::numeric * COALESCE(se.quantity,1)) / NULLIF(SUM(COALESCE(se.quantity,1)),0) AS avg_equipment_quality,
    json_agg(DISTINCT se.equipment_id ORDER BY se.equipment_id) AS equipment_ids
  FROM squad_equipment se
  JOIN equipment e ON e.equipment_id = se.equipment_id
  GROUP BY se.squad_id
),

combat_skill_rows AS (
  SELECT DISTINCT ds.dwarf_id, ds.skill_id
  FROM dwarf_skills ds
  JOIN skills s ON s.skill_id = ds.skill_id
  WHERE s.category ILIKE 'combat' OR s.skill_type ILIKE 'combat'
),
skill_hist AS (
  SELECT
    ds.dwarf_id, ds.skill_id, ds.level, ds.date,
    FIRST_VALUE(ds.level) OVER (PARTITION BY ds.dwarf_id, ds.skill_id ORDER BY ds.date ROWS BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING) AS level_first,
    LAST_VALUE(ds.level)  OVER (PARTITION BY ds.dwarf_id, ds.skill_id ORDER BY ds.date ROWS BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING) AS level_last
  FROM dwarf_skills ds
  JOIN combat_skill_rows csr ON csr.dwarf_id = ds.dwarf_id AND csr.skill_id = ds.skill_id
),
-- должен рассчитать дельту
skill_delta AS (
  SELECT
    sm.squad_id,
    AVG(GREATEST(level_last - level_first, 0))::numeric AS avg_combat_skill_improvement
  FROM skill_hist h
  JOIN squad_members sm ON sm.dwarf_id = h.dwarf_id  -- считаем по тем, кто состоял когда-либо
  GROUP BY sm.squad_id
),

pairs AS (
  SELECT
    sb.squad_id,
    sb.report_id,
    CASE WHEN sb.outcome = 'victory' THEN 1.0 ELSE 0.0 END AS win_flag,
    COALESCE((
      SELECT SUM(st.effectiveness::numeric)
      FROM squad_training st
      WHERE st.squad_id = sb.squad_id
        AND st.date >= sb.date - INTERVAL '30 days'
        AND st.date <  sb.date
    ), 0.0) AS train_effect_30d
  FROM squad_battles sb
),
train_corr AS (
  SELECT
    squad_id,
    corr(win_flag, train_effect_30d) AS training_battle_correlation
  FROM pairs
  GROUP BY squad_id
),

-- не осилил, надо посмотреть примерное решение.

```
