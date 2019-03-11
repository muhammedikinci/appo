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

### Commands

| Command Name | Status |
| ------ | ------ |
| add-rule  | OK |
| load-rule | NONE YET |
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
