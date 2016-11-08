<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.ProductUnclassified" CodeBehind="ProductUnclassified.aspx.cs" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
	<div class="dataarea mainwidth databody">
     <div class="title">
          <em><img src="../images/03.gif" width="32" height="32" /></em>
          <h1>商品扩展分类</h1>
          <span>查看店铺中所有商品所属的分类及扩展分类，您可以在此对商品设置扩展分类，也可以批量转移商品的主分类及扩展分类</span>
      </div>
	  <div class="datalist">
		<!--搜索-->
		<div class="searcharea clearfix" style="padding:10px 0px 3px 0px;">
			<ul>
				<li><span>商品名称：</span><span>
				    <asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput" /></span></li>
				<li>
					<abbr class="formselect">
						<Hi:ProductCategoriesDropDownList ID="dropCategories" IsUnclassified="true" NullToDisplay="--请选择商品分类--" runat="server" width="150" />
					</abbr>
				</li>
                <li style="margin-left:12px;">
					<abbr class="formselect">
						<Hi:ProductTypeDownList ID="dropType" runat="server" NullToDisplay="--请选择商品类型--"  width="153"/>
					</abbr>
				</li>
				<li >
					<abbr class="formselect">
						<Hi:ProductLineDropDownList ID="dropLines" IsShowNoset="true" runat="server" NullToDisplay="--请选择产品线--"  width="153"/>
					</abbr>
				</li>
				<li>
					<abbr class="formselect">
						<Hi:BrandCategoriesDropDownList runat="server" ID="dropBrandList" NullToDisplay="--请选择商品品牌--" width="153"></Hi:BrandCategoriesDropDownList>
					</abbr>
				</li>
				</ul>
		</div>
		<div class="searcharea clearfix" style="padding:3px 0px 10px 0px;">
		    <ul>
                <li><span>商家编码：</span><span><asp:TextBox ID="txtSKU" Width="74" runat="server" CssClass="forminput" /></span></li>
                
		        <li><span>添加时间：</span></li>
		        <li>
		            <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" cssclass="forminput" />
		            <span class="Pg_1010">至</span>
		            <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" cssclass="forminput" />
		        </li>
		        <li><asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="searchbutton"/></li>
		    </ul>
		</div>
			<div class="advanceSearchArea ">
		<!--预留显示高级搜索项区域-->
		</div>
		<!--结束-->

		<div class="functionHandleArea clearfix">
			<!--分页功能-->
			<div class="pageHandleArea">
				<ul>
					<li class="paginalNum"><span>每页显示数量：</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
				</ul>
			</div>
			<div class="pageNumber">
				<div class="pagination">
                <UI:Pager runat="server" ShowTotalPages="false" ID="pager1" />
            </div>
			</div>
			<!--结束-->

			<div class="blank8 clearfix"></div>
			<div class="batchHandleArea">
				<ul>
					<li class="batchHandleButton">
					<span class="signicon"></span>
					<span class="allSelect"><a href="javascript:void(0)" onclick="SelectAll()">全选</a></span>
					<span class="reverseSelect"><a href="javascript:void(0)" onclick="ReverseSelect()">反选</a></span>
                    <span class="delete"><Hi:ImageLinkButton ID="btnDelete" runat="server" Text="删除" IsShow="true" DeleteMsg="确定要把商品移入回收站吗？" /></span>
				</ul>
			</div>
		</div>
		
		<!--数据列表区域-->
	    <UI:Grid ID="grdProducts" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="ProductId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns>   
                        <asp:TemplateField ItemStyle-Width="30px" HeaderText="选择" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                                <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("ProductId") %>'  />
                            </itemtemplate>
                        </asp:TemplateField>                       
                        <asp:TemplateField HeaderText="商品名称" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <div style="float:left; margin-right:10px;">
                                <a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank">
                                        <Hi:ListImage ID="ListImage2"  runat="server" DataField="ThumbnailUrl40"/>      
                                 </a> 
                                 </div>
                                 <div>
                                <span class="Name"><a href='<%#"../../ProductDetails.aspx?ProductId="+Eval("ProductId")%>' target="_blank"><%# Eval("ProductName") %></a></span>
                                <span class="colorC">商家编码：<%# Eval("ProductCode")%></span>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="所属分类" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <div>
                                    <nobr><span style="font-size:13px;" >主分类：<abbr style=" color:Blue"><asp:Literal runat="server" ID="litMainCategory" /></abbr></span></nobr>
                                </div>
                                <div>
                                    <nobr><span style="font-size:13px;">扩展分类：<abbr style=" color:Blue"><asp:Literal runat="server" ID="litExtendCategory" /></abbr></span></nobr>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="设置扩展分类" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <Hi:ProductCategoriesDropDownList ID="dropAddToCategories" 
                                    runat="server" AutoPostBack="true" />
                            </ItemTemplate>
                        </asp:TemplateField> 
                                     
            </Columns>
        </UI:Grid>
		  <div class="blank12 clearfix"></div>
      </div>
		<div class="bottomPageNumber clearfix">
			<div class="pageNumber">
				<div class="pagination">
				<UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
				</div>
			</div>
			</div>
		<div class="blank12 clearfix"></div>
	  <div  class=" br_search" style=" border-bottom:1px #ddd solid;height:80px;">
            <div style="float:left;width:450px;">
	        <div class="searcharea clearfix">
        	<ul style="width:100%">
				<li><span><b>移动商品到分类：</b></span>
					<span class="formselect">
						<Hi:ProductCategoriesDropDownList ID="dropMoveToCategories" runat="server" />
					</span>
				</li>
                <li>
                    <asp:Button runat="server" ID="btnMove" Text="转移主类" CssClass="submit_queding"/>
                </li>
			</ul>
          </div>
          <div class="colorD ">批量转移商品的主类或者将商品转移到未分类，在转移以前请先选择要转移的商品。</div>
	        </div>
	 
        <div style="float:right; width:450px;">
	  <div class="searcharea clearfix">
        	<ul style="width:100%">
				<li><span><b>设置商品到扩展分类：</b></span>
					<span class="formselect">
						        <Hi:ProductCategoriesDropDownList ID="dropAddToAllCategories" runat="server"  />
					</span>
				</li>
                <li>
                    <asp:Button runat="server" ID="btnSetCategories" Text="设置扩展" CssClass="submit_queding"/>
                </li>
			</ul>
          </div>
          <div class="colorD ">批量设置商品的扩展分类，在设置以前请先选择要设置扩展分类的商品。</div>
	  
	  </div>
		</div>
        
   </div> 
   
   <script>
function GetProductId(){
        var v_str = "";

        $("input[type='checkbox'][name='CheckBoxGroup']:checked").each(function(rowIndex, rowItem){
            v_str += $(rowItem).attr("value") + ",";
        });
        
        if(v_str.length == 0){
            alert("请选择商品");
            return "";
        }
        return v_str.substring(0, v_str.length - 1);        
    }
</script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

