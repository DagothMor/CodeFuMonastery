
# 2
```sql
SELECT
    d.dwarf_id,
    d.name,
    d.age,
    d.profession,
    JSON_OBJECT(
        'skill_ids', COALESCE((
            SELECT JSON_ARRAYAGG(DISTINCT ds.skill_id)
            FROM dwarf_skills ds
            WHERE ds.dwarf_id = d.dwarf_id
        ), JSON_ARRAY()),
        'assignment_ids', COALESCE((
            SELECT JSON_ARRAYAGG(DISTINCT da.assignment_id)
            FROM dwarf_assignments da
            WHERE da.dwarf_id = d.dwarf_id
              AND da.start_date <= CURRENT_DATE
              AND (da.end_date IS NULL OR da.end_date > CURRENT_DATE)
        ), JSON_ARRAY()),
        'squad_ids', COALESCE((
            SELECT JSON_ARRAYAGG(DISTINCT sm.squad_id)
            FROM squad_members sm
            WHERE sm.dwarf_id = d.dwarf_id
              AND sm.exit_date IS NULL
        ), JSON_ARRAY()),
        'equipment_ids', COALESCE((
            SELECT JSON_ARRAYAGG(DISTINCT de.equipment_id)
            FROM dwarf_equipment de
            WHERE de.dwarf_id = d.dwarf_id
        ), JSON_ARRAY())
    ) AS related_entities
FROM dwarves d;
```


# 3 

```sql
SELECT
    w.workshop_id,
    w.name,
    w.type,
    w.quality,
    JSON_OBJECT(
        'craftsdwarf_ids', COALESCE((
            SELECT JSON_ARRAYAGG(DISTINCT wc.dwarf_id)
            FROM workshop_craftsdwarves wc
            WHERE wc.workshop_id = w.workshop_id
        ), JSON_ARRAY()),
        'project_ids', COALESCE((
            SELECT JSON_ARRAYAGG(DISTINCT p.project_id)
            FROM projects p
            WHERE p.workshop_id = w.workshop_id
              AND p.status = 'active'
        ), JSON_ARRAY()),
        'input_material_ids', COALESCE((
            SELECT JSON_ARRAYAGG(DISTINCT wm.material_id)
            FROM workshop_materials wm
            WHERE wm.workshop_id = w.workshop_id
              AND wm.is_input = TRUE
        ), JSON_ARRAY()),
        'output_product_ids', COALESCE((
            SELECT JSON_ARRAYAGG(DISTINCT wp.product_id)
            FROM workshop_products wp
            WHERE wp.workshop_id = w.workshop_id
        ), JSON_ARRAY())
    ) AS related_entities
FROM workshops w;
```


# 4

```sql
SELECT
    s.squad_id,
    s.name,
    s.formation_type,
    s.leader_id,
    JSON_OBJECT(
        'member_ids', COALESCE((
            SELECT JSON_ARRAYAGG(DISTINCT sm.dwarf_id)
            FROM squad_members sm
            WHERE sm.squad_id = s.squad_id
        ), JSON_ARRAY()),
        'equipment_ids', COALESCE((
            SELECT JSON_ARRAYAGG(DISTINCT se.equipment_id)
            FROM squad_equipment se
            WHERE se.squad_id = s.squad_id
        ), JSON_ARRAY()),
        'operation_ids', COALESCE((
            SELECT JSON_ARRAYAGG(DISTINCT so.operation_id)
            FROM squad_operations so
            WHERE so.squad_id = s.squad_id
        ), JSON_ARRAY()),
        'training_schedule_ids', COALESCE((
            SELECT JSON_ARRAYAGG(DISTINCT st.schedule_id)
            FROM squad_training st
            WHERE st.squad_id = s.squad_id
        ), JSON_ARRAY()),
        'battle_report_ids', COALESCE((
            SELECT JSON_ARRAYAGG(DISTINCT sb.report_id)
            FROM squad_battles sb
            WHERE sb.squad_id = s.squad_id
        ), JSON_ARRAY())
    ) AS related_entities
FROM military_squads s;

```
