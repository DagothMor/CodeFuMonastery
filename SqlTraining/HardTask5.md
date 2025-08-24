```sql
WITH
-- воркшопы
w AS (
  SELECT w.workshop_id, w.name AS workshop_name, w.type AS workshop_type
  FROM workshops w
),

-- воркшопперы
crafts AS (
  SELECT
    wc.workshop_id,
    COUNT(DISTINCT wc.dwarf_id) AS num_craftsdwarves,
    json_agg(DISTINCT wc.dwarf_id ORDER BY wc.dwarf_id) AS craftsdwarf_ids
  FROM workshop_craftsdwarves wc
  GROUP BY wc.workshop_id
),

prod AS (
  SELECT
    wp.workshop_id,
    SUM(wp.quantity) AS total_quantity_produced,
    SUM((p.value) * (wp.quantity)) AS total_production_value,
    MIN(wp.production_date) AS min_prod_date,
    MAX(wp.production_date) AS max_prod_date,
    COUNT(DISTINCT wp.production_date) AS prod_days,
    json_agg(DISTINCT wp.product_id ORDER BY wp.product_id) AS product_ids
  FROM workshop_products wp
  JOIN products p ON p.product_id = wp.product_id
  GROUP BY wp.workshop_id
),

-- материалы
mats AS (
  SELECT
    wm.workshop_id,
    SUM(CASE WHEN wm.is_input THEN wm.quantity ELSE 0 END)
     AS input_material_qty,
    json_agg(DISTINCT CASE WHEN wm.is_input THEN wm.material_id END
             ORDER BY CASE WHEN wm.is_input THEN wm.material_id END)
      FILTER (WHERE wm.is_input) AS material_ids
  FROM workshop_materials wm
  GROUP BY wm.workshop_id
),

-- проекты
projs AS (
  SELECT
    p.workshop_id,
    json_agg(DISTINCT p.project_id ORDER BY p.project_id) AS project_ids
  FROM projects p
  GROUP BY p.workshop_id
),

activity AS (
  SELECT
    pr.workshop_id,
    COALESCE(pr.prod_days, 0) AS prod_days,
    GREATEST(COALESCE((pr.max_prod_date - pr.min_prod_date) + 1, 0), 0)::int AS active_days
  FROM prod pr
),

last_skill_per_dwarf AS (
  SELECT
    wc.workshop_id,
    ds.level::numeric AS level
  FROM workshop_craftsdwarves wc
  JOIN LATERAL (
    SELECT dsi.level
    FROM dwarf_skills dsi
    WHERE dsi.dwarf_id = wc.dwarf_id
    ORDER BY dsi.date DESC
    LIMIT 1
  ) ds ON TRUE
),
avg_skill AS (
  SELECT workshop_id, AVG(level) AS average_craftsdwarf_skill
  FROM last_skill_per_dwarf
  GROUP BY workshop_id
),

pairs AS (
  SELECT
    wp.workshop_id,
    wp.production_date,
    AVG(ls.level)::numeric AS avg_skill_at_date,
    AVG(p.quality::numeric) AS product_quality_at_date
  FROM workshop_products wp
  JOIN products p ON p.product_id = wp.product_id
  JOIN workshop_craftsdwarves wc ON wc.workshop_id = wp.workshop_id
  LEFT JOIN LATERAL (
    SELECT ds.level
    FROM dwarf_skills ds
    WHERE ds.dwarf_id = wc.dwarf_id
      AND ds.date <= wp.production_date
    ORDER BY ds.date DESC
    LIMIT 1
  ) ls ON TRUE
  GROUP BY wp.workshop_id, wp.production_date
),
corrs AS (
  SELECT
    workshop_id,
    corr(product_quality_at_date, avg_skill_at_date) AS skill_quality_correlation
  FROM pairs
  GROUP BY workshop_id
)

SELECT
  w.workshop_id,
  w.workshop_name,
  w.workshop_type,

  COALESCE(c.num_craftsdwarves, 0)                         AS num_craftsdwarves,
  COALESCE(p.total_quantity_produced, 0)::numeric          AS total_quantity_produced,
  COALESCE(p.total_production_value, 0)::numeric           AS total_production_value,

  ROUND(
    COALESCE(p.total_quantity_produced, 0)
    / NULLIF(a.active_days, 0)::numeric
  , 2)                                                     AS daily_production_rate,

  ROUND(
    COALESCE(p.total_production_value, 0)
    / NULLIF(m.input_material_qty, 0)
  , 2)                                                     AS value_per_material_unit,

  ROUND(
    COALESCE(a.prod_days, 0)::numeric
    / NULLIF(a.active_days, 0)
    * 100
  , 2)                                                     AS workshop_utilization_percent,

  ROUND(
    COALESCE(p.total_quantity_produced, 0)
    / NULLIF(m.input_material_qty, 0)
  , 2)                                                     AS material_conversion_ratio,

  ROUND(COALESCE(s.average_craftsdwarf_skill, 0), 2)       AS average_craftsdwarf_skill,

  ROUND(COALESCE(cr.skill_quality_correlation, 0), 2)      AS skill_quality_correlation,

  json_build_object(
    'craftsdwarf_ids', COALESCE(c.craftsdwarf_ids, '[]'::json),
    'product_ids',     COALESCE(p.product_ids,     '[]'::json),
    'material_ids',    COALESCE(m.material_ids,    '[]'::json),
    'project_ids',     COALESCE(pr.project_ids,    '[]'::json)
  ) AS related_entities

FROM w
LEFT JOIN crafts   c  ON c.workshop_id = w.workshop_id
LEFT JOIN prod     p  ON p.workshop_id = w.workshop_id
LEFT JOIN mats     m  ON m.workshop_id = w.workshop_id
LEFT JOIN projs    pr ON pr.workshop_id = w.workshop_id
LEFT JOIN activity a  ON a.workshop_id = w.workshop_id
LEFT JOIN avg_skill s ON s.workshop_id = w.workshop_id
LEFT JOIN corrs    cr ON cr.workshop_id = w.workshop_id
ORDER BY total_production_value DESC NULLS LAST;
```
