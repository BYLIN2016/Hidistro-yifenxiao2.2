namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class HeaderMenuTypeRadioButtonList : RadioButtonList
    {
        protected override void Render(HtmlTextWriter writer)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("<input id=\"radHeaderMenuType_1\" type=\"radio\" name=\"radHeaderMenuType\" value=\"{0}\" />系统页面", 1);
            builder.AppendFormat("<input id=\"radHeaderMenuType_2\" type=\"radio\" name=\"radHeaderMenuType\" value=\"{0}\" />商品搜索链接", 2);
            builder.AppendFormat("<input id=\"radHeaderMenuType_3\" type=\"radio\" name=\"radHeaderMenuType\" value=\"{0}\" />自定义链接", 3);
            writer.Write(builder.ToString());
        }
    }
}

