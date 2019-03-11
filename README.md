# appo
Content Alert

### Usage

Search in web content

```sh
// search content
>> appo add-rule sc http://localhost Happy! 1000
>> appo start 0
```

Detect changes

```sh
// changes content
>> appo add-rule cc http://localhost 1000
>> appo start 0
```

```sh
// All Rules
>> appo rules

// Remove rule
>> appo remove-rule 0

// Remove all rules
>> appo remove-all
```

Load rules from file.
```sh
>>appo load
>>7 Rules loaded successfully

>>appo rules

│ RuleID: 0   │ URL: http://localhost   │ RefreshTime: 1000   │ isRunning: 0   │

│ RuleID: 1   │ URL: http://localhost   │ RefreshTime: 1000   │ isRunning: 0   │

│ RuleID: 2   │ URL: http://localhost   │ RefreshTime: 1000   │ isRunning: 0   │

│ RuleID: 3   │ URL: http://localhost   │ RefreshTime: 1000   │ isRunning: 0   │

│ RuleID: 4   │ URL: http://localhost   │ RefreshTime: 1000   │ isRunning: 0   │

│ RuleID: 5   │ URL: http://localhost   │ RefreshTime: 1000   │ isRunning: 0   │

│ RuleID: 6   │ URL: http://localhost   │ RefreshTime: 1000   │ isRunning: 0   │

──────────────────────────────────────
Total Rules: 7
Running Now: 0
Idled: 7
──────────────────────────────────────
>>
```

Save rules and load
```sh
>>appo add-rule cc http://localhost 1000
Success: New rule added. Rule Id 0
>>appo save
>>Rules saved successfully

>>appo rules

│ RuleID: 0   │ URL: http://localhost   │ RefreshTime: 1000   │ isRunning: 0   │

──────────────────────────────────────
Total Rules: 1
Running Now: 0
Idled: 1
──────────────────────────────────────
>>appo remove-all
Success: 0 rule is stopped.
Rules removed
>>appo load
>>1 Rules loaded successfully

>>appo rules

│ RuleID: 0   │ URL: http://localhost   │ RefreshTime: 1000   │ isRunning: 0   │

──────────────────────────────────────
Total Rules: 1
Running Now: 0
Idled: 1
──────────────────────────────────────
>>
```

### Commands

| Command Name | Status |
| ------ | ------ |
| add-rule  | OK |
| load | OK |
| save | OK |
| hide | NONE YET |
| stop <<rule_id>> | OK |
| start <<rule_id>> | OK |
| remove-rule <<rule_id>> | OK |
| remove-all | OK |
| rules | OK |
| start all | OK |
| stop all | OK |

### Features

#### Rule Action Options

| Action | Status |
| ------ | ------ |
| Open program  | NONE YET |
| Send Mail | NONE YET |
| Print Alert | NONE YET |
| Print message in command prompt/terminal | OK |

#### Working On SystemTray

| OS | Status |
| ------ | ------ |
| Linux  | NONE YET |
| Windows | NONE YET |
| MacOs | NONE YET |
