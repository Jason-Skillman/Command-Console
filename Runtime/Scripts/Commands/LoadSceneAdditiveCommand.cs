namespace JasonSkillman.Console.Commands {
    using UnityEngine;
    using UnityEngine.SceneManagement;
    
    public class LoadSceneAdditiveCommand : ICommand {
        public string Label => "load-scene-additive";

        public string[] SuggestedArgs(string[] args) {
            return new[] {
                "<scene-name>"
            };
        }

        public void Action(string[] args) {
            string sceneName = args[0];

            //Load the scene if it exists
            if(Application.CanStreamedLevelBeLoaded(sceneName)) {
                SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                CommandConsole.Instance.Log($"Loaded scene {sceneName} (Additive)");
            } else {
                CommandConsole.Instance.LogError($"Scene {sceneName} does not exist");
            }
        }
    }
}
