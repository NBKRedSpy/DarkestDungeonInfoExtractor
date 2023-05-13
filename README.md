# DarkestDungeonInfoExtractor

Extracts Attack info from game files.

Creates a .csv file for all of the \*.info.darkest files in the specified folder and subfolders.
The output is currently only the skill launch and target position info.  Contains the positions as well as a pip version of those positions.

Example:

```
MonsterName,Skill,IsMultiTarget, IsFriendlyTarget, TargetGlyph, TargetPos1,TargetPos2,TargetPos3,TargetPos4,LaunchGlyph, LaunchPos1,LaunchPos2,LaunchPos3,LaunchPos4
ancestor_big_D,"ancestor_fuse_two",False,False,◉◉◉◉,True,True,True,True,◉◉◉◉,True,True,True,True
ancestor_big_D,"ancestor_fuse_all",True,False,◉-◉-◉-◉,True,True,True,True,◉◉◉◉,True,True,True,True
ancestor_big_D,"ancestor_contemplate",False,False,◉◉◉◉,True,True,True,True,◉◉◉◉,True,True,True,True
```

Usage
```
DarkestDungeonInfoExtractor 1.0.0
Copyright (C) 2023 DarkestDungeonInfoExtractor

ERROR(S):
  A required value not bound to option name is missing.

  --help          Display this help screen.

  --version       Display version information.

  value pos. 0    Required. The monsters folder of Darkest Dungeon.  Will be
                  [DarkestDungeon folder]\monsters

  value pos. 1    Required. The name of the output file


```
