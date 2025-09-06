```sql
WITH
-- 1) Транзакции (знак баланса по направлению)
txn AS (
  SELECT
    tt.transaction_id,
    tt.caravan_id,
    tt.date::date AS dt,
    tt.value::numeric AS value_abs,
    CASE
      WHEN tt.balance_direction ILIKE 'profit' OR tt.balance_direction ILIKE 'export'
        THEN tt.value::numeric
      WHEN tt.balance_direction ILIKE 'loss'   OR tt.balance_direction ILIKE 'import'
        THEN -tt.value::numeric
      ELSE 0::numeric
    END AS value_signed
  FROM trade_transactions tt
),

car AS (
  SELECT c.caravan_id, c.civilization_type, c.fortress_id
  FROM caravans c
),

-- Баланс торговли с каждой цивилизацией за все время 
all_time AS (
  SELECT
    COUNT(DISTINCT car.civilization_type)            AS total_trading_partners,
    COALESCE(SUM(txn.value_abs), 0)::numeric         AS all_time_trade_value,
    COALESCE(SUM(txn.value_signed), 0)::numeric      AS all_time_trade_balance
  FROM txn
  JOIN car ON car.caravan_id = txn.caravan_id
),

caravan_trade AS (
  SELECT
    car.civilization_type,
    txn.caravan_id,
    SUM(txn.value_abs)    AS caravan_trade_value,
    SUM(txn.value_signed) AS caravan_trade_balance
  FROM txn
  JOIN car ON car.caravan_id = txn.caravan_id
  GROUP BY car.civilization_type, txn.caravan_id
),

caravan_dipl AS (
  SELECT
    de.caravan_id,
    SUM(COALESCE(de.relationship_change,0))::numeric AS rel_change_sum
  FROM diplomatic_events de
  GROUP BY de.caravan_id
),

-- Корреляцию между торговлей и дипломатическими отношениями
by_civ AS (
  SELECT
    ct.civilization_type,
    COUNT(DISTINCT ct.caravan_id)                                       AS total_caravans,
    COALESCE(SUM(ct.caravan_trade_value),0)::numeric                    AS total_trade_value,
    COALESCE(SUM(ct.caravan_trade_balance),0)::numeric                  AS trade_balance,
    corr(cd.rel_change_sum, ct.caravan_trade_value)                     AS diplomatic_correlation,
    json_agg(DISTINCT ct.caravan_id ORDER BY ct.caravan_id)             AS caravan_ids
  FROM caravan_trade ct
  LEFT JOIN caravan_dipl cd ON cd.caravan_id = ct.caravan_id
  GROUP BY ct.civilization_type
),

by_civ_labeled AS (
  SELECT
    civilization_type,
    total_caravans,
    total_trade_value,
    trade_balance,
    CASE WHEN trade_balance > 0 THEN 'Favorable'
         WHEN trade_balance < 0 THEN 'Unfavorable'
         ELSE 'Neutral' END AS trade_relationship,
    ROUND(COALESCE(diplomatic_correlation,0), 2) AS diplomatic_correlation,
    caravan_ids
  FROM by_civ
),

-- Импортные зависимости 
imports_raw AS (
  SELECT
    cg.caravan_id,
    cg.material_type,
    cg.quantity::numeric                 AS qty,
    cg.price_fluctuation::numeric        AS price_fluctuation,
    p.material_id                        AS resource_id
  FROM caravan_goods cg
  LEFT JOIN products p ON p.product_id = cg.original_product_id
  WHERE cg.type ILIKE 'import'
),

imports_agg AS (
  SELECT
    material_type,
    SUM(qty)                                                AS total_imported,
    COUNT(DISTINCT resource_id)                             AS import_diversity,
    COALESCE(AVG(price_fluctuation),0)                      AS avg_fluct,
    json_agg(DISTINCT resource_id) FILTER (WHERE resource_id IS NOT NULL) AS resource_ids,
    SUM(qty) * (1 + COALESCE(AVG(price_fluctuation),0))     AS dependency_score
  FROM imports_raw
  GROUP BY material_type
),

-- Эффективность экспорта мастерских
exports_goods AS (
  SELECT
    cg.quantity::numeric         AS ex_qty,
    cg.value::numeric            AS ex_price,
    p.product_id,
    p.type                       AS product_type,
    p.value::numeric             AS base_value,
    p.workshop_id,
    w.type                       AS workshop_type
  FROM caravan_goods cg
  JOIN products  p ON p.product_id  = cg.original_product_id
  JOIN workshops w ON w.workshop_id = p.workshop_id
  WHERE cg.type ILIKE 'export'
),

produced AS (
  SELECT
    wp.workshop_id,
    p.type AS product_type,
    SUM(wp.quantity)::numeric AS produced_qty
  FROM workshop_products wp
  JOIN products p ON p.product_id = wp.product_id
  GROUP BY wp.workshop_id, p.type
),

exports_agg AS (
  SELECT
    eg.workshop_type,
    eg.product_type,
    ARRAY_AGG(DISTINCT eg.workshop_id)                 AS workshop_ids,
    SUM(eg.ex_qty)                                     AS exported_qty,
    SUM(eg.ex_qty * eg.ex_price)                       AS export_revenue,
    SUM(eg.ex_qty * eg.base_value)                     AS export_base_value
  FROM exports_goods eg
  GROUP BY eg.workshop_type, eg.product_type
),

export_effect AS (
  SELECT
    ea.workshop_type,
    ea.product_type,
    ROUND(100 * COALESCE(ea.exported_qty,0)
          / NULLIF(SUM(pr.produced_qty),0), 1)         AS export_ratio,
    ROUND(COALESCE(ea.export_revenue,0)
          / NULLIF(COALESCE(ea.export_base_value,0),0), 2) AS avg_markup,
    (SELECT json_agg(DISTINCT wid ORDER BY wid)
     FROM unnest(ea.workshop_ids) AS wid)              AS workshop_ids
  FROM exports_agg ea
  LEFT JOIN produced pr
    ON pr.product_type = ea.product_type
   AND pr.workshop_id = ANY(ea.workshop_ids)
  GROUP BY ea.workshop_type, ea.product_type, ea.workshop_ids, ea.exported_qty, ea.export_revenue, ea.export_base_value
)

-- Эффективность экспорта продукции мастерских
-- не смог составить.


civilization_trade_data AS (
  SELECT json_agg(
           json_build_object(
             'civilization_type',      b.civilization_type,
             'total_caravans',         b.total_caravans,
             'total_trade_value',      b.total_trade_value,
             'trade_balance',          b.trade_balance,
             'trade_relationship',     b.trade_relationship,
             'diplomatic_correlation', b.diplomatic_correlation,
             'caravan_ids',            COALESCE(b.caravan_ids, '[]'::json)
           )
           ORDER BY b.total_trade_value DESC
         ) AS data
  FROM by_civ_labeled b
),

critical_import_dependencies AS (
  SELECT json_agg(
           json_build_object(
             'material_type',    i.material_type,
             'dependency_score', ROUND(i.dependency_score, 1),
             'total_imported',   i.total_imported,
             'import_diversity', i.import_diversity,
             'resource_ids',     COALESCE(i.resource_ids, '[]'::json)
           )
           ORDER BY i.dependency_score DESC
         ) AS data
  FROM imports_agg i
),

export_effectiveness_json AS (
  SELECT json_agg(
           json_build_object(
             'workshop_type', ee.workshop_type,
             'product_type',  ee.product_type,
             'export_ratio',  ee.export_ratio,
             'avg_markup',    ee.avg_markup,
             'workshop_ids',  COALESCE(ee.workshop_ids, '[]'::json)
           )
           ORDER BY ee.export_ratio DESC
         ) AS data
  FROM export_effect ee
)

-- Итого
SELECT json_build_object(
  'total_trading_partners', at.total_trading_partners,
  'all_time_trade_value',   at.all_time_trade_value,
  'all_time_trade_balance', at.all_time_trade_balance,

  'civilization_data', json_build_object(
    'civilization_trade_data', COALESCE(ctd.data, '[]'::json)
  ),

  'critical_import_dependencies', json_build_object(
    'resource_dependency', COALESCE(cid.data, '[]'::json)
  ),

  'export_effectiveness', json_build_object(
    'export_effectiveness', COALESCE(ee.data, '[]'::json)
  )
) AS trade_analysis_json
FROM all_time at
LEFT JOIN civilization_trade_data ctd      ON TRUE
LEFT JOIN critical_import_dependencies cid ON TRUE
LEFT JOIN export_effectiveness_json ee     ON TRUE

```
