using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditor;

public class A1Test {

    //private string sceneAssetPath = "Assets/Assignments/WarlocksGame.unity";
    private string sceneName = "WarlocksGame";

    private string notesAssetPath = "Assets/Assignment/notes.txt";
    private string notesFail = "[Insert";


    [Test]
    public void Test_AssignmentNotes() {
        TextAsset notes = AssetDatabase.LoadAssetAtPath<TextAsset>(notesAssetPath);
        Assert.IsFalse(notes.text.Contains(notesFail), "Fill in your assignment notes");
    }

    [Test]
    public void Test_CorrectSceneLoaded() {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        Assert.True(scene.isLoaded, string.Format("Scene {0} not loaded", sceneName));
    }

    [Test]
    public void Test_Units() {
        List<Unit> units = new List<Unit>(Object.FindObjectsOfType<Unit>());

        Assert.LessOrEqual(2, units.Count, "Not enough units in scene");

        foreach(Unit unit in units) {
            Test_Unit(unit);
        }
    }

    private void Test_Unit(Unit unit) {
        Assert.NotNull(unit.Health, string.Format("Health is null {0}", unit.Name));
        Assert.NotNull(unit.Controller, string.Format("Controller is null {0}", unit.Name));
        Assert.NotNull(unit.Owner, string.Format("Owner is null {0}", unit.Name));
    }

    [Test]
    public void Test_Players() {
        List<PlayerController> playerControllers = new List<PlayerController>(Object.FindObjectsOfType<PlayerController>());
        List<Unit> units = new List<Unit>(Object.FindObjectsOfType<Unit>());

        Assert.LessOrEqual(2, playerControllers.Count, "Not enough players in scene");
        Assert.AreEqual(1, playerControllers.FindAll((pc) => pc.Player.IsLocal).Count, "There should be 1 local player");

        foreach(PlayerController playerController in playerControllers) {
            Test_PlayerController(playerController);
        }

        foreach (PlayerController playerController in playerControllers) {
            List<IUnit> playerUnits = playerController.Units;
            Assert.Less(0, playerUnits.Count, string.Format("Player {0} has no units", playerController.Player.Name));
        }
    }

    private void Test_PlayerController(PlayerController playerController) {
        Assert.NotNull(playerController.Player, "Player Controller doesn't have a player");
    }

    [Test]
    public void Test_Environment() {

        Terrain terrain = Object.FindObjectOfType<Terrain>();
        Assert.NotNull(terrain, "Terrain is missing");

        AreaDamageOverTime lava = Object.FindObjectOfType<AreaDamageOverTime>();
        Assert.NotNull(lava, "Lava is missing");

        Island island = Object.FindObjectOfType<Island>();
        Assert.NotNull(island, "Island is missing");

    }

    [Test]
    public void Test_Game() {

        Game game = Object.FindObjectOfType<Game>();
        Assert.NotNull(game, "Game is missing");

    }

    [Test]
    public void Test_AppIntegration() {

        Scene scene = SceneManager.GetSceneByName(sceneName);
        List<ISceneLoader> sceneLoaders = scene.FindObjects<ISceneLoader>();
        Assert.Greater(sceneLoaders.Count, 0, "Scene does not have a scene loader.");

    }
	
}
