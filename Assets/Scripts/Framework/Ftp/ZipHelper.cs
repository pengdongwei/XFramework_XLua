using ICSharpCode.SharpZipLib.Zip;
public class ZipHelper{
	public static void ZipFolder(string zipFile, string folder){
		FastZip zip = new FastZip();
		zip.CreateZip(zipFile, folder, true, "");
	}

	public static void UnZipFolder(string folder, string zipFile){
		FastZip zip = new FastZip();
		zip.ExtractZip(folder, zipFile, "");
	}

	public static string[] ZipFolders(){
		return new string[]{ "assetbundle/data", "assetbundle/effect" };
	}
}