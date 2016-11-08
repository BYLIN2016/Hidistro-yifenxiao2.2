<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpecificationView.ascx.cs" Inherits="Hidistro.UI.Web.Admin.product.ascx.SpecificationView" %>
<%@ Import Namespace="Hidistro.Core"%>
 <%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
 <%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
   
      <div class="content">
	
            <UI:Grid ID="grdSKU" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="AttributeId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns>
                    <asp:TemplateField HeaderText="规格名称" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="20%">
                        <ItemTemplate>
                            规格名[<asp:Literal runat="server" ID="litUseAttributeImage" Text='<%# Eval("UseAttributeImage") %>' />]：
		                    <Hi:HtmlDecodeTextBox ID="txtSKUName" runat="server" Text='<%# Eval("AttributeName") %>' Width="70px" />
		                    <asp:Literal ID="lblDisplaySequence" runat="server" Text='<%#Eval("DisplaySequence") %>' Visible=false></asp:Literal>
		                    <asp:LinkButton ID="lbtnAdd" Text="修改" runat="server" CommandName="saveSKUName" CommandArgument='<%# Container.DataItemIndex  %>' />
                        </ItemTemplate>
                    </asp:TemplateField>  
                    
                      <asp:TemplateField HeaderText="规格值"  HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="45%">
                        <ItemTemplate>
                            <asp:Repeater ID="rptSKUValue" runat="server" DataSource='<%# Eval("AttributeValues") %>'>
   		                               <ItemTemplate>
   		                           <span class="SKUValue">
   		                                <span class="span1"><Hi:SKUImage ID="SKUImage1" runat="server" CssClass="a_none" ImageUrl='<%# Eval("ImageUrl")%>' ValueStr='<%# Eval("ValueStr")%>' /></span>
                                        <span class="span2"><a href="javascript:void(0)" onclick="deleteSKUValue(this, '<%# Eval("ValueId")%>', '<%# Eval("ImageUrl")%>');">删除</a></span>
                                    </span>
   		                        </ItemTemplate>
   		                    </asp:Repeater>
                        </ItemTemplate>
                    </asp:TemplateField>   
                                   
                    <UI:SortImageColumn HeaderText="排序"  ReadOnly="true" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="7%"/>
                     <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="20%">
                         <ItemStyle CssClass="spanD spanN" />
                         <ItemTemplate>
	                         <span class="submit_tiajia"><a href="javascript:void(0)" onclick="ShowAddSKUValueDiv('<%# Eval("AttributeId") %>','<%# Eval("UseAttributeImage") %>', '<%# Eval("AttributeName") %>');">添加规格值</a></span>	                         	                        
 	                         <span class="submit_bianji"><asp:HyperLink  ID="lkbViewAttribute" runat="server" Text="编辑" NavigateUrl='<%# Globals.GetAdminAbsolutePath(string.Format("product/EditSpecificationValues.aspx?TypeId={0}&AttributeId={1}&UseAttributeImage={2}",Eval("TypeId"),Eval("AttributeId"),Eval("UseAttributeImage")))%>' ></asp:HyperLink></span> 
 	                         <span class="submit_dalata"><Hi:ImageLinkButton runat="server" ID="lbtnDelete" CommandName="delete" IsShow="true" DeleteMsg="当前操作将彻底删该除规格及下属的所有规格值，确定吗？" Text="删除" /></span>

                         </ItemTemplate>
                     </asp:TemplateField> 
                                     
            </Columns>
        </UI:Grid>
    </div>
        
        <div class="Pg_15">
          <input type="button" onclick="AddSkuDiv()" name="button" id="button" value="添加新规格" class="submit_bnt3"/>
        </div>


<%--添加新的规格--%>
<div id="addSKU" style="display:none;">
    <div class="frame-content">
        <p><span class="frame-span frame-input90">规格名称：<em >*</em></span><asp:TextBox ID="txtName" CssClass="forminput" runat="server"></asp:TextBox></p>
        <b id="ctl00_contentHolder_specificationView_txtNameTip">规格名称长度在1至30个字符之间</b>
        <span class="frame-span frame-input90">显示类型：</span> <Hi:UseAttributeImageRadioButtonList runat="server" ID="radIsImage" style="display:inline;" />
    </div>
</div>




   <div style="display:none">
           <asp:Button ID="btnCreate" runat="server" CssClass="submit_DAqueding" Text="添加新规格"/>
   </div>

<script type="text/javascript" language="javascript">
    var formtype = "";
     //判断规格值
    

     //添加新规格
     function AddSkuDiv() {
         //addSKU
         formtype = "addsku";
         DialogShow("添加新规格", "addskuwin", "addSKU", "ctl00_contentHolder_specificationView_btnCreate");
     }

     //添加规格值
     function ShowAddSKUValueDiv(attributeId, useAttributeImage, attributename) {
         var pathurl = "product/SkuValue.aspx?action=add&attributeId=" + attributeId + "&useImg=" + useAttributeImage;
        var title = "添加" + attributename + "的规格值";
        if (useAttributeImage == "True") {
            DialogFrame(pathurl, title, 420, 200);
        } else {
            DialogFrame(pathurl, title, 440, 180);
        }
   
    }



    function validatorForm() {
        arrytext = null;
        var skuname=$("#ctl00_contentHolder_specificationView_txtName").val().replace(/\s/g,"");
        if (skuname == "") {
            alert("请输入规格名称");
            return false;
        }
        if (skuname.length < 1 || skuname.length>30) {
            alert("规格名称长度在1至30个字符之间");
            return false;
        }
        $radioId = $("input[type='radio'][name='ctl00$contentHolder$specificationView$radIsImage']:checked")[0];
        setArryText($radioId.id, "true");
        setArryText('ctl00_contentHolder_specificationView_txtName', skuname);
        return true;
    }

    function deleteSKUValue(obj, valueId, imageUrl) {
        $.ajax({
            url: "AddSpecification.aspx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: { ValueId: valueId, ImageUrl: imageUrl, isCallback: "true" },
            async: false,
            success: function(data) {
                if (data.Status == "true") 
                {
                   location.reload();
                }
                else {
                    ShowMsg("此规格值有商品在使用，删除失败", false);
                }
            }
        });
    }
    
   
</script>

