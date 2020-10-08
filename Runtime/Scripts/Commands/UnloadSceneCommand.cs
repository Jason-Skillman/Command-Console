using UnityEngine.SceneManagement;

namespace DebugCommandConsole.Commands {
    public class UnloadSceneCommand : ICommand {
        public string Label => "unload-scene";

        public string[] SuggestedArgs(string[] args) {
            return new[] {
                "<scene-name>"
            };
        }

        public void Action(string[] args) {
            string sceneName = args[0];

            //Loop through all of the currently loaded scenes
            for(int i = 0; i < SceneManager.sceneCount; i++) {
                Scene scene = SceneManager.GetSceneAt(i);

                //Check if the scene name is currently loaded
                if(sceneName.Equals(scene.name)) {
                    SceneManager.UnloadSceneAsync(sceneName);
                    CommandConsole.Instance.Log($"Unloaded scene {sceneName}");
                    return;
                }
            }

            //If no loaded scene macthed the scene to unload
            CommandConsole.Instance.LogError($"Scene {sceneName} is not currently loaded");
        }
    }
}
