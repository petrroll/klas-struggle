# Notes for future us

## FireBaseConnector.cs
Could/Should be reworked to use RealtimeDatabse/CloudStorage.
- \+ Native Unity SDK.
- \+ Meant for similar usecases.
- \- Not JSON native (just a bit more work to handle files, rows).

 Other opportunities:
 * Firebase Auth: Authentication to identify "your" wheats (?) && mitigate bad actors.
 * Notify when another user adds wheat to field -> grows on your field as well (? Cloud functions || ? polling).
   * Semi-relatime view of field other than one snapshot -> handles race conditions of 2 whats on 1 spot.

## Async/Await
Migrate all custom .NET's `Task` based `async`/`await` to [UnityAsync](https://github.com/muckSponge/UnityAsync) or [UniTask](https://github.com/Cysharp/UniTask).
- \+ No allocation ([no longer] relevant on Unity 2018?](https://github.com/Demigiant/dotween/issues/387#issuecomment-608371554)).
- \+ UniTask: integration with DOTween ([no longer relevant?](https://github.com/Demigiant/dotween/issues/387)).
- \- More third party code, it works fine now.
- \- Performance is currently not an issue.

