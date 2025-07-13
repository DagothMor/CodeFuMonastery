
```
1

select * from dwarves d

JOIN squads s ON d.squad_id = s.squad_id;
```

```
2

SELECT *
FROM dwarves
WHERE profession = 'miner'
  AND squad_id IS NULL;
```


```
3

SELECT *
FROM tasks
WHERE status = 'pending'
  AND priority = (
      SELECT MAX(priority)
      FROM tasks
      WHERE status = 'pending'
  );
```

```
4

SELECT
    d.dwarf_id,
    d.name,
    COUNT(i.item_id) AS items_count
FROM
    dwarves d
    JOIN items i ON d.dwarf_id = i.owner_id
GROUP BY
    d.dwarf_id, d.name;
```

```
5

SELECT
    s.squad_id,
    s.name AS squad_name,
    COUNT(d.dwarf_id) AS dwarves_count
FROM
    squads s
    LEFT JOIN dwarves d ON s.squad_id = d.squad_id
GROUP BY
    s.squad_id, s.name;
```

```
6

SELECT
    d.profession,
    COUNT(t.task_id) AS tasks_count
FROM
    tasks t
    JOIN dwarves d ON t.assigned_to = d.dwarf_id
WHERE
    t.status IN ('pending', 'in_progress')
GROUP BY
    d.profession
ORDER BY
    tasks_count DESC
LIMIT 1;

```

```
7

SELECT
    i.type,
    AVG(d.age) AS avg_owner_age
FROM
    items i
    JOIN dwarves d ON i.owner_id = d.dwarf_id
GROUP BY
    i.type;
```

```
8

SELECT
    d.*
FROM
    dwarves d
    LEFT JOIN items i ON d.dwarf_id = i.owner_id
WHERE
    d.age > (SELECT AVG(age) FROM dwarves)
    AND i.item_id IS NULL;
```
