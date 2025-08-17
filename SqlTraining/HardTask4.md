```sql

WITH
base AS (
  SELECT
    e.expedition_id,
    e.destination,
    e.status,
    e.departure_date,
    e.return_date
  FROM expeditions e
),

m AS (
  SELECT
    em.expedition_id,
    COUNT(*) AS members_total,
    SUM(CASE WHEN em.survived THEN 1 ELSE 0 END) AS members_survived
  FROM expedition_members em
  GROUP BY em.expedition_id
),
survival AS (
  SELECT
    expedition_id,
    CASE WHEN members_total > 0
      THEN 100.0 * members_survived::numeric / members_total
      ELSE NULL
    END AS survival_rate
  FROM m
),
artifacts AS (
  SELECT
    ea.expedition_id,
    SUM(ea.value)::numeric AS artifacts_value
  FROM expedition_artifacts ea
  GROUP BY ea.expedition_id
),

sites AS (
  SELECT
    es.expedition_id,
    COUNT(DISTINCT es.site_id)::numeric AS discovered_sites
  FROM expedition_sites es
  GROUP BY es.expedition_id
),
encounters AS (
  SELECT
    ec.expedition_id,
    SUM(CASE WHEN ec.outcome = 'favorable' THEN 1 ELSE 0 END) AS favorable_cnt,
    SUM(CASE WHEN ec.outcome IN ('favorable','unfavorable') THEN 1 ELSE 0 END) AS considered_cnt
  FROM expedition_creatures ec
  GROUP BY ec.expedition_id
),
encounter_rates AS (
  SELECT
    expedition_id,
    CASE WHEN considered_cnt > 0
      THEN 100.0 * favorable_cnt::numeric / considered_cnt
      ELSE NULL
    END AS encounter_success_rate
  FROM encounters
),
skill_before_dt AS (
  SELECT
    b.expedition_id,
    em.dwarf_id,
    ds.skill_id,
    MAX(ds.date) AS dt
  FROM base b
  JOIN expedition_members em ON em.expedition_id = b.expedition_id
  JOIN dwarf_skills ds
    ON ds.dwarf_id = em.dwarf_id
   AND ds.date < b.departure_date
  GROUP BY b.expedition_id, em.dwarf_id, ds.skill_id
),
skill_before AS (
  SELECT
    sbd.expedition_id,
    sbd.dwarf_id,
    sbd.skill_id,
    ds.experience::numeric AS exp_before
  FROM skill_before_dt sbd
  JOIN dwarf_skills ds
    ON ds.dwarf_id = sbd.dwarf_id
   AND ds.skill_id  = sbd.skill_id
   AND ds.date      = sbd.dt
),
skill_after_dt AS (
  SELECT
    b.expedition_id,
    em.dwarf_id,
    ds.skill_id,
    MIN(ds.date) AS dt
  FROM base b
  JOIN expedition_members em ON em.expedition_id = b.expedition_id
  JOIN dwarf_skills ds
    ON ds.dwarf_id = em.dwarf_id
   AND ds.date >= COALESCE(b.return_date, CURRENT_DATE)
  GROUP BY b.expedition_id, em.dwarf_id, ds.skill_id
),
skill_after AS (
  SELECT
    sad.expedition_id,
    sad.dwarf_id,
    sad.skill_id,
    ds.experience::numeric AS exp_after
  FROM skill_after_dt sad
  JOIN dwarf_skills ds
    ON ds.dwarf_id = sad.dwarf_id
   AND ds.skill_id  = sad.skill_id
   AND ds.date      = sad.dt
),
skill_improve AS (
  SELECT
    x.expedition_id,
    SUM(GREATEST(COALESCE(x.exp_after, 0) - COALESCE(x.exp_before, 0), 0))::numeric AS skill_improvement
  FROM (
    SELECT
      COALESCE(sa.expedition_id, sb.expedition_id) AS expedition_id,
      COALESCE(sa.dwarf_id,     sb.dwarf_id)       AS dwarf_id,
      COALESCE(sa.skill_id,     sb.skill_id)       AS skill_id,
      sa.exp_after,
      sb.exp_before
    FROM skill_after sa
    FULL OUTER JOIN skill_before sb
      ON sb.expedition_id = sa.expedition_id
     AND sb.dwarf_id      = sa.dwarf_id
     AND sb.skill_id      = sa.skill_id
  ) x
  GROUP BY x.expedition_id
),
duration AS (
  SELECT
    expedition_id,
    (COALESCE(return_date, CURRENT_DATE) - departure_date) AS expedition_duration
  FROM base
),
  SELECT
    b.expedition_id,
    b.destination,
    b.status
  FROM base b
  LEFT JOIN survival        s  ON s.expedition_id  = b.expedition_id
  LEFT JOIN artifacts       a  ON a.expedition_id  = b.expedition_id
),
norms AS (
  SELECT
    MAX(artifacts_value)                         AS max_artifacts,
    MAX(discovered_sites)                        AS max_sites,
    MAX(GREATEST(skill_improvement, 0))          AS max_skill,
    MAX(expedition_duration)                     AS max_duration
  FROM metrics
)

FROM metrics m
CROSS JOIN norms n
```
