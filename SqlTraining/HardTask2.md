# 1

```
select * from squads s

where s.leader_id is null
```

# 2

```
select * from dwarves d

where d.age > 150 and d.profession = 'Warrior'
```

# 3

```
select distinct * from Dwarves d join Items i on d.dwarf_id = i.owner_id where i.type = 'weapon';
```

# 4
```
select d.name, t.status, count(t.task_id) as task_count
from Dwarves d
left join Tasks t on d.dwarf_id = t.assigned_to
group by d.name, t.status;
```

# 5
```
select t.task_id, t.description, t.status
from Tasks t
join Dwarves d on t.assigned_to = d.dwarf_id
join Squads s on d.squad_id = s.squad_id
where s.name = 'Guardians';
```

# 6

```
select d1.name as dwarf_name, d2.name as relative_name, r.relationship from Relationships r join Dwarves d1 on r.dwarf_id = d1.dwarf_id join Dwarves d2 on r.related_to = d2.dwarf_id;
```
