using UnityEditor;

public class RenameAndLoad : AssetPostprocessor
{
	void OnPreprocessAnimation()
	{
		if (assetPath.Contains("Scout Anims but for real this time.fbx"))
		{
			ModelImporter modelImporter = assetImporter as ModelImporter;

			ModelImporterClipAnimation[] clipsToRename = modelImporter.defaultClipAnimations;

			for (int i = 0; i < clipsToRename.Length; i++)
			{
				clipsToRename[i].name = clipsToRename[i].name.Replace("scout.qc_skeleton|@", "");
			}
			for (int i = 0; i < clipsToRename.Length; i++)
			{
				clipsToRename[i].name = clipsToRename[i].name.Replace("scout|", "");
			}

			modelImporter.clipAnimations = clipsToRename;
		}
	}
}
