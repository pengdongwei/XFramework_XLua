//*************************************************************************
//	创建日期:	2017-06-2
//	文件名称:	UIExport.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Text;
using System.IO;

public class UIExport {

    #region 导出 UIVIEW  LUA 代码相关
    private const string EXPORT_LUA_PATH = "Lua/UI/View";
    private const string EXPORT_LUA_CONTROLLER_PATH = "Lua/UI/Controller";

    private const string UI_BUTTON = "btn_";
    private const string UI_TRANSFORM = "trans_";
    private const string UI_GAMEOBJECT = "go_";
    private const string UI_TOGGLE = "tog_";
    private const string UI_TEXT = "text_";
    private const string UI_IMAGE = "img_";
    private const string UI_INPUT = "input_";
    private const string UI_SLIDER = "slider_";
    private const string UI_ITEM = "item_";
    private const string UI_DROPDOWN = "dropdown_";

    /// <summary>
    /// 把对应的UIPanel转换成相应的VIEW脚本
    /// </summary>
    [MenuItem("XTool/XLua/导出Lua UI")]
    static private void export2UILua()
    {
        GameObject selectobj = Selection.activeGameObject;
        if (selectobj != null)
        {
            Transform transRoot = selectobj.transform;

            StringBuilder resultBuilder = new StringBuilder();
            resultBuilder.Append("local _V = {}\r\n");
            export2LuaByGameObject(selectobj, resultBuilder, false);

            //导出item下的内容
            List<GameObject> lstItem = new List<GameObject>();
            findUIByStartName(transRoot, UI_ITEM, lstItem);
            for (int i = 0; i < lstItem.Count; i++)
            {
                export2LuaByGameObject(lstItem[i], resultBuilder, true);
            }

            resultBuilder.Append("return _V\r\n");

            //Debug.LogError(resultBuilder.ToString());

            string outfilePath = Application.dataPath + "/" + EXPORT_LUA_PATH;
            if (!Directory.Exists(outfilePath))
            {
                Directory.CreateDirectory(outfilePath);
            }
            string fileName = selectobj.name + "View";
            outfilePath = outfilePath + "/" + fileName + ".lua";
            UTF8Encoding utf8 = new UTF8Encoding(false); //  保存为 无 Bom Utf-8格式 
            File.WriteAllText(outfilePath, resultBuilder.ToString(), utf8);
        }
    }


	[MenuItem("XTool/XLua/导出Lua Ctrl")]
    static private void export2UIController()
    {
        GameObject selectobj = Selection.activeGameObject;
        if (selectobj == null) return;

        string assetpath = AssetDatabase.GetAssetPath(selectobj);
        string dirName = null;
        if (assetpath != null)
        {
            string dirpath = Path.GetDirectoryName(assetpath);
            //Debug.LogError(dirpath);
            DirectoryInfo _dirinfo = new DirectoryInfo(dirpath);
            dirName = _dirinfo.Name;
        }

        string template_path = "Assets/Editor/UITool/UIControllerTemplate.lua";
        string sel_ui_name = selectobj.name;
        string content = File.ReadAllText(template_path);
        content = content.Replace("{CLASS_NAME}", sel_ui_name);
        string outfilePath = Application.dataPath + "/" + EXPORT_LUA_CONTROLLER_PATH;
        if (dirName != null)
        {
            outfilePath += "/" + dirName;
        }
        if (!Directory.Exists(outfilePath)) Directory.CreateDirectory(outfilePath);
        outfilePath += "/" + sel_ui_name + ".lua";
        UTF8Encoding utf8 = new UTF8Encoding(false); //  保存为 无 Bom Utf-8格式 
        File.WriteAllText(outfilePath, content, utf8);
        Debug.Log("done > " + outfilePath);
    }


    static private void export2LuaByGameObject(GameObject _root, StringBuilder _resultBuilder, bool _isitem)
    {
        if (_isitem)
        {
            _resultBuilder.Append(string.Format("_V.{0} = function( _gameObject )\r\n", _root.name));
            _resultBuilder.Append("    local trans = _gameObject.transform\r\n");
            _resultBuilder.Append("    local t = {}\r\n");
        }
        else
        {
            _resultBuilder.Append("_V.Init = function( _gameObject )\r\n");
            _resultBuilder.Append("    local trans = _gameObject.transform\r\n");
            _resultBuilder.Append("    local t = {}\r\n");
        }


        Transform transRoot = _root.transform;

        List<GameObject> lstBtn = new List<GameObject>();
        List<GameObject> lstTrans = new List<GameObject>();
        List<GameObject> lstGo = new List<GameObject>();
        List<GameObject> lstTog = new List<GameObject>();
        List<GameObject> lstText = new List<GameObject>();
        List<GameObject> lstImage = new List<GameObject>();
        List<GameObject> lstInput = new List<GameObject>();
        List<GameObject> lstSlider = new List<GameObject>();
        List<GameObject> lstItem = new List<GameObject>();
        List<GameObject> lstDropdown = new List<GameObject>();

        findUIByStartName(transRoot, UI_BUTTON, lstBtn);
        findUIByStartName(transRoot, UI_TRANSFORM, lstTrans);
        findUIByStartName(transRoot, UI_GAMEOBJECT, lstGo);
        findUIByStartName(transRoot, UI_TOGGLE, lstTog);
        findUIByStartName(transRoot, UI_TEXT, lstText);
        findUIByStartName(transRoot, UI_IMAGE, lstImage);
        findUIByStartName(transRoot, UI_INPUT, lstInput);
        findUIByStartName(transRoot, UI_SLIDER, lstSlider);
        findUIByStartName(transRoot, UI_ITEM, lstItem);
        findUIByStartName(transRoot, UI_DROPDOWN, lstDropdown);
        //检查是否有出错的
        checkGameObject(_root, lstBtn, "Button");
        checkGameObject(_root, lstTrans, "Transform");
        checkGameObject(_root, lstTog, "Toggle");
        checkGameObject(_root, lstText, "Text");
        checkGameObject(_root, lstImage, "Image");
        checkGameObject(_root, lstInput, "InputField");
        checkGameObject(_root, lstSlider, "Slider");
        checkGameObject(_root, lstDropdown, "Dropdown");
        //输出 
        for (int i = 0; i < lstGo.Count; i++)
        {
            string path = getPath(_root, lstGo[i]);
            bool isUnderItem = checkIsUnderItem(lstGo[i]);
            if (!_isitem && isUnderItem)
            {
                //当前build的obj是在item下，而且当前是导出全部的情况
                continue;
            }
            else
            {
                if (checkPath(path))
                {
                    _resultBuilder.Append(string.Format("    t.{0} = utils.GetGameObject( trans, '{1}' )\r\n", lstGo[i].name, path));
                }
            }
        }

        for (int i = 0; i < lstBtn.Count; i++)
        {
            string path = getPath(_root, lstBtn[i]);
            bool isUnderItem = checkIsUnderItem(lstBtn[i]);
            if (!_isitem && isUnderItem)
            {
                //当前build的obj是在item下，而且当前是导出全部的情况
                continue;
            }
            else
            {
                if (checkPath(path))
                {
                    _resultBuilder.Append(string.Format("    t.{0} = utils.GetGameObject( trans, '{1}' )\r\n", lstBtn[i].name, path));
                }
            }

        }

        for (int i = 0; i < lstTrans.Count; i++)
        {
            string path = getPath(_root, lstTrans[i]);
            bool isUnderItem = checkIsUnderItem(lstTrans[i]);
            if (!_isitem && isUnderItem)
            {
                //当前build的obj是在item下，而且当前是导出全部的情况
                continue;
            }
            else
            {
                if (checkPath(path))
                {
                    _resultBuilder.Append(string.Format("    t.{0} = trans:Find( '{1}' )\r\n", lstTrans[i].name, path));
                }
            }
        }

        for (int i = 0; i < lstTog.Count; i++)
        {
            string path = getPath(_root, lstTog[i]);
            bool isUnderItem = checkIsUnderItem(lstTog[i]);
            if (!_isitem && isUnderItem)
            {
                //当前build的obj是在item下，而且当前是导出全部的情况
                continue;
            }
            else
            {
                if (checkPath(path))
                {
                    _resultBuilder.Append(string.Format("    t.{0} = LTUtils.GetComponentByPath( trans, '{1}', 'Toggle' )\r\n", lstTog[i].name, path));
                }
            }
        }

        for (int i = 0; i < lstText.Count; i++)
        {
            string path = getPath(_root, lstText[i]);
            bool isUnderItem = checkIsUnderItem(lstText[i]);
            if (!_isitem && isUnderItem)
            {
                //当前build的obj是在item下，而且当前是导出全部的情况
                continue;
            }
            else
            {
                if (checkPath(path))
                {
                    _resultBuilder.Append(string.Format("    t.{0} = LTUtils.GetComponentByPath( trans, '{1}', 'Text' )\r\n", lstText[i].name, path));
                }
            }
        }
        for (int i = 0; i < lstImage.Count; i++)
        {
            string path = getPath(_root, lstImage[i]);
            bool isUnderItem = checkIsUnderItem(lstImage[i]);
            if (!_isitem && isUnderItem)
            {
                //当前build的obj是在item下，而且当前是导出全部的情况
                continue;
            }
            else
            {
                if (checkPath(path))
                {
                    _resultBuilder.Append(string.Format("    t.{0} = LTUtils.GetComponentByPath( trans, '{1}', 'Image' )\r\n", lstImage[i].name, path));
                }
            }
        }

        for (int i = 0; i < lstInput.Count; i++)
        {
            string path = getPath(_root, lstInput[i]);
            bool isUnderItem = checkIsUnderItem(lstInput[i]);
            if (!_isitem && isUnderItem)
            {
                //当前build的obj是在item下，而且当前是导出全部的情况
                continue;
            }
            else
            {
                if (checkPath(path))
                {
                    _resultBuilder.Append(string.Format("    t.{0} = LTUtils.GetComponentByPath( trans, '{1}', 'InputField' )\r\n", lstInput[i].name, path));
                }
            }
        }

        for (int i = 0; i < lstSlider.Count; i++)
        {
            string path = getPath(_root, lstSlider[i]);
            bool isUnderItem = checkIsUnderItem(lstSlider[i]);
            if (!_isitem && isUnderItem)
            {
                //当前build的obj是在item下，而且当前是导出全部的情况
                continue;
            }
            else
            {
                if (checkPath(path))
                {
                    _resultBuilder.Append(string.Format("    t.{0} = LTUtils.GetComponentByPath( trans, '{1}', 'Slider' )\r\n", lstSlider[i].name, path));
                }
            }
        }

        for (int i = 0; i < lstItem.Count; i++)
        {
            string path = getPath(_root, lstItem[i]);
            bool isUnderItem = checkIsUnderItem(lstItem[i]);
            if (!_isitem && isUnderItem)
            {
                //当前build的obj是在item下，而且当前是导出全部的情况
                continue;
            }
            else
            {
                if (checkPath(path))
                {
                    _resultBuilder.Append(string.Format("    t.{0} = utils.GetGameObject( trans, '{1}' )\r\n", lstItem[i].name, path));
                }
            }
        }
        for (int i = 0; i < lstDropdown.Count; i++)
        {
            string path = getPath(_root, lstDropdown[i]);
            bool isUnderItem = checkIsUnderItem(lstDropdown[i]);
            if (!_isitem && isUnderItem)
            {
                //当前build的obj是在item下，而且当前是导出全部的情况
                continue;
            }
            else
            {
                if (checkPath(path))
                {
                    _resultBuilder.Append(string.Format("    t.{0} = LTUtils.GetComponentByPath( trans, '{1}','Dropdown' )\r\n", lstDropdown[i].name, path));
                }
            }
        }
        _resultBuilder.Append("    return t\r\n");
        _resultBuilder.Append("end\r\n");
    }


    static private void findUIByStartName(Transform _transParent, string _startName, List<GameObject> _lstObj)
    {
        int childcount = _transParent.childCount;
        if (childcount == 0) return;
        for (int i = 0; i < childcount; i++)
        {
            Transform _trans = _transParent.GetChild(i);
            GameObject _go = _trans.gameObject;
            if (_go.name.StartsWith(_startName)) {
                _lstObj.Add(_go);
            }
            findUIByStartName(_trans, _startName, _lstObj);
        }
    }

    static private void checkGameObject( GameObject _parentObj,  List<GameObject> _lst, string _componentName)
    {
        int count = _lst.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject obj = _lst[i];
            Component script = obj.GetComponent(_componentName);
            if (script == null)
            {
                Debug.LogError("Object没拿到脚本问题 > " + obj.name);
                string path = getPath(_parentObj, obj);
                Debug.LogError(path);
            }
        }
    }

    static private string getPath(GameObject _parent, GameObject _obj)
    {
        string path = "";
        Transform rootparent = _parent.transform;
        List<GameObject> lstparents = new List<GameObject>();
        Transform objparent = _obj.transform.parent;
        while (objparent != rootparent)
        {
            lstparents.Add(objparent.gameObject);
            objparent = objparent.parent;
            if (objparent == null)
            {
                break;
            }
        }
        lstparents.Insert(0, _obj);
        int len = lstparents.Count;
        path = lstparents[len - 1].gameObject.name;
        for (int i = len - 2; i >= 0; i--)
        {
            path += "/" + lstparents[i].gameObject.name;
        }
        return path;
    }
    /// <summary>
    ///  判断obj是否在item下边
    /// </summary>
    static private bool checkIsUnderItem(GameObject _obj)
    {
        bool bRet = false;
        Transform objParent = _obj.transform.parent;
        while (objParent != null)
        {
            GameObject obj = objParent.gameObject;
            if (obj.name.StartsWith(UI_ITEM))
            {
                bRet = true;
                break;
            }
            objParent = objParent.parent;
        }
        return bRet;
    }

    static private bool checkPath(string _path)
    {
        //if (_path.Contains(" "))
        //{
        //    Debug.LogError("命名存在问题(包含空格),请检查:" + _path);
        //    return false;
        //}
        return true;
    }

    #endregion
}
