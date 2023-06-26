using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;
using BIMBOX.Revit.Toolkit.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMBOX.Revit.Entity
{
    public class BOX_Material
    {
        public BOX_Material(Material material) 
        {
            Material = material;

            _name = material.Name;

            Color = material.Color;

            AppearanceColor = GetAppearanceColor();

        }
        private string _name;
        private Autodesk.Revit.DB.Color _color;


        public Material Material { get; private set; }

        public Document Document { get => Material.Document; }
        
        public string Name 
        { 
            get=> _name;
            set 
            {
                _name = value;
                Document.NewTransaction("修改名称", () => Material.Name = value);
            }
        }

        
        public Autodesk.Revit.DB.Color Color 
        { 
            get => _color;
            set 
            {
                _color = value;
                Document.NewTransaction("修改颜色", ()=>  Material.Color = value);
            }
        }

        public Autodesk.Revit.DB.Color AppearanceColor { get; set; }

        private Autodesk.Revit.DB.Color GetAppearanceColor()
        {
            ElementId id = Material.AppearanceAssetId;
            if (id != null&&id.IntegerValue != -1)
            {
                AppearanceAssetElement appearanceAssetElement = Document.GetElement(id) as AppearanceAssetElement;
                Asset asset = appearanceAssetElement.GetRenderingAsset();
                AssetPropertyDoubleArray4d property = (AssetPropertyDoubleArray4d)asset?.FindByName("generic_diffuse");
                return property?.GetValueAsColor();
            }
            return null;
        }

    }

}