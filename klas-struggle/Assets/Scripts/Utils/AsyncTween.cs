using DG.Tweening;
using System.Threading.Tasks;

namespace Assets.Scripts.Utils
{
    public static class AsyncTween
    {
        public static Task Await(this Tweener tweener)
        {
            // crude allocate-y solution, perfectly fine for this use-case
            var tcs = new TaskCompletionSource<object>();
            tweener.OnComplete(() => tcs.SetResult(null));
            return tcs.Task;
        }
    }
}