#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class RyzeyBetterURPWelcomeWindow : EditorWindow
{
    private int selectedTab = 0;
    private Vector2 scrollPos;

    private readonly string[] tabs = new string[]
    {
        "Welcome",
        "Known Issues",
        "Support"
    };

    private const string WindowShownKey = "RyzeyBetterURP_WelcomeWindowShown";

    [MenuItem("RyzeyBetterURP/Show Welcome Window")]
    public static void ShowWindow()
    {
        var window = GetWindow<RyzeyBetterURPWelcomeWindow>(true, "Welcome to Ryzey.Better.URP Beta", true);
        window.minSize = new Vector2(450, 280);
        window.ShowModalUtility();
    }

    public static void ShowWindowOnce()
    {
        if (!EditorPrefs.GetBool(WindowShownKey, false))
        {
            ShowWindow();
            EditorPrefs.SetBool(WindowShownKey, true);
        }
    }

    private void OnGUI()
    {
        selectedTab = GUILayout.Toolbar(selectedTab, tabs);

        EditorGUILayout.Space();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        switch (selectedTab)
        {
            case 0: // Welcome
                DrawWelcomeTab();
                break;

            case 1: // Known Issues
                DrawIssuesTab();
                break;

            case 2: // Support
                DrawSupportTab();
                break;
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();
        if (GUILayout.Button("Close"))
        {
            Close();
        }
    }

    private void DrawWelcomeTab()
    {
        EditorGUILayout.LabelField("Thanks for Buying Ryzey.Better.URP Beta", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField(
            "This is my first ever URP package, so some features might have bugs.\n" +
            "Don't worry, fixes are on the way!\n\n" +
            "Enjoy customizing your project with Better URP settings.",
            EditorStyles.wordWrappedLabel);
    }

    private void DrawIssuesTab()
    {
        EditorGUILayout.LabelField("Known Issues", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("- Some Bloom intensity values might reset unexpectedly.\n" +
                                   "- Depth of Field Gaussian mode may cause minor flickering.\n" +
                                   "- Chromatic Aberration might be subtle on some devices.",
                                   EditorStyles.wordWrappedLabel);
    }

    private void DrawSupportTab()
    {
        EditorGUILayout.LabelField("Support & Contact", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField(
            "If you encounter any issues or want to suggest features, please contact:\n\n" +
            "Email: support@ryzey.com\n" +
            "Discord: Ryzey#1234\n" +
            "GitHub: https://github.com/ryzey/better-urp\n\n" +
            "Thank you for your support!",
            EditorStyles.wordWrappedLabel);
    }
}
#endif
