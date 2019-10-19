## Idea: 
- GameDesigner creates Stages:n-Stage:n-Questions:n-Answers hierarchy 
	- stage, question, and answer ids are all set automatically
- Selected decisions are routed through ControllerProxyDec : Decision to WheatController
	- Wheat controller gets stage, question, and answer ids and reacts

# Hierarchy:
- Stages
	- Stage1: explicitly assigned: nextStage, requires Decision script on the same gameObject
		- Question1
			- Answer1
			- Answer2
		- Question2
	- Stage2

## Structure:
- Answer:
	- No editor-set fields
	- Holds reference to it's Decision handler: inherited from stage
	- Has automatically set AnswerID that influences key-mapping, ...

	- Responsible for detecting that it has been selected
		- Can only be selected if `Question.Stage.ReadyForAnswers`
		- Selected -> calls .Decide(this) on Decision

Question:
	- No editor-set fields
	- Holds reference to Stage, gathers all children Answers, has Id
	- Inits all children Answers

Stage: 
	- void ActivateStage(): activates stage, inits stuff, ...
	- Task FinishStage(): returns Task that represents 
QuestionStage : Stage
	- void ActivateStage(): activates stage, inits stuff, ...
		- Sets appropriate GameObjects as Active
		- Sets ReadyForAnswers = true
		- Randomly selects a question to activate
		- Fades-in activated stuff
	- Task FinishStage(): returns Task that represents 
		- Sets ReadyForAnswers = false
		- Starts FadeOut of children nodes
		- Disables current gameObject
		- NextStage?.ActivateStage();
	- Init(int stageId):
		- Gathers children Questions, inits them
		- Initializes Decision (gathers it as component)
		- Potentially activates on start 
	- bool ActiveOnStart
	- bool FadeIn
	- Stage NextStage
FinalStage:
	- void ActivateStage(): activates stage, inits stuff, ...
		- Saves generated wheatState
		- Activates next scene

Stages: 
	- Gathers & inits all children Stages






