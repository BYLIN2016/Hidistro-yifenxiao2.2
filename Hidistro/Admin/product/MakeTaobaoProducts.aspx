<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="MakeTaobaoProducts.aspx.cs" Inherits="Hidistro.UI.Web.Admin.product.MakeTaobaoProducts" Title="�ޱ���ҳ" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth databody">
		  <div class="title">
  <em><img src="../images/03.gif" width="32" height="32" /></em>
  <h1>�����Ա�����</h1>
  <span>����Ʒ�����������Ա����ݷ������Ա����̣�ʵ���Ա�������Ʒ���̳���Ʒ��ͨ</span>
</div>

<div class="datalist">
<div class="searcharea clearfix">
			<ul>
				<li><span>��Ʒ���ƣ�</span><span><asp:TextBox ID="txtSearchText" runat="server" CssClass="forminput"  /></span></li>
				<li style="margin-left:13px;">
					<abbr class="formselect">
						<Hi:ProductCategoriesDropDownList ID="dropCategories" runat="server" NullToDisplay="--��ѡ����Ʒ����--" width="153" />
					</abbr>
				</li>
				<li>
					<abbr class="formselect">
						<Hi:ProductLineDropDownList ID="dropLines" runat="server" NullToDisplay="--��ѡ���Ʒ��--" width="153"/>
					</abbr>
				</li>
			
                <li><span>�̼ұ��룺</span><span> <asp:TextBox ID="txtSKU" Width="110" runat="server" CssClass="forminput" /></span></li>
			</ul>
		</div>
		<div class="searcharea clearfix">
		    <ul>
		        <li><span>���ʱ�䣺</span></li>
		        <li style="padding-left:0px;">
		            <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" runat="server" cssclass="forminput"   />
		            <span class="Pg_1010">��</span>
		            <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate" cssclass="forminput"   />
		        </li>
		        <li><asp:Button ID="btnSearch" runat="server" Text="��ѯ" CssClass="searchbutton"/></li>
		    </ul>
		</div>
         <div class="functionHandleArea clearfix">
			<!--��ҳ����-->
			<div class="pageHandleArea">
				<ul>
					<li class="paginalNum"><span>ÿҳ��ʾ������</span><UI:PageSize runat="server" ID="hrefPageSize" /></li>
				</ul>
			</div>
			<div class="pageNumber">
				<div class="pagination">
                <UI:Pager runat="server" ShowTotalPages="false" ID="pager1" />
                </div>
            
			</div>
		<div class="filterClass"> <span><b>����״̬��</b></span> <span class="formselect">
                <asp:DropDownList ID="dpispub" runat="server"
            onselectedindexchanged="dpispub_SelectedIndexChanged" AutoPostBack="True">
                    <asp:ListItem Value="-1">-��ѡ��-</asp:ListItem>
                    <asp:ListItem Value="1">�Ѵ���</asp:ListItem>
                    <asp:ListItem Value="0">δ����</asp:ListItem>
                </asp:DropDownList>
                </span>
		      </div>
			
		</div>
	    <UI:Grid runat="server" ID="grdProducts" Width="100%" AllowSorting="true" ShowOrderIcons="true" GridLines="None" DataKeyNames="ProductId"
                    SortOrder="Desc"  AutoGenerateColumns="false" HeaderStyle-CssClass="table_title">
                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="30px" HeaderText="ѡ��" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                                <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("ProductId") %>' />
                            </itemtemplate>
                        </asp:TemplateField>                               
                        <asp:BoundField HeaderText="����" DataField="DisplaySequence" ItemStyle-Width="40px" HeaderStyle-CssClass="td_right td_left" />
                        <asp:TemplateField ItemStyle-Width="35%" HeaderText="��Ʒ" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                            <div style="float:left; margin-right:10px;">
                                <a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank">
                                        <Hi:ListImage ID="ListImage1"  runat="server" DataField="ThumbnailUrl40"/>      
                                 </a> 
                                 </div>
                                 <div style="float:left;">
                                 <span class="Name"> <a href='<%#"../../ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank"><%# Eval("ProductName") %></a></span>
                                  <span class="colorC">�̼ұ��룺<%# Eval("ProductCode") %></span>
                                 </div>
                         </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="���"  ItemStyle-Width="100" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                             <asp:Label ID="lblStock" runat="server" Text='<%# Eval("Stock") %>' Width="25"></asp:Label>
                          </itemtemplate>
                        </asp:TemplateField>
                        <Hi:MoneyColumnForAdmin HeaderText=" �г���" ItemStyle-Width="80"  DataField="MarketPrice" HeaderStyle-CssClass="td_right td_left" />
                        <Hi:MoneyColumnForAdmin HeaderText="�ɱ���" ItemStyle-Width="80"  DataField="CostPrice" HeaderStyle-CssClass="td_right td_left"  />        
                        <Hi:MoneyColumnForAdmin HeaderText="һ�ڼ�" ItemStyle-Width="80"  DataField="SalePrice" HeaderStyle-CssClass="td_right td_left"  />   
                        <Hi:MoneyColumnForAdmin HeaderText="�ɹ���" ItemStyle-Width="80"  DataField="PurchasePrice" HeaderStyle-CssClass="td_right td_left"  />    
                        <asp:TemplateField HeaderText="����״̬"  ItemStyle-Width="100" HeaderStyle-CssClass="td_right td_left">
                            <itemtemplate>
                             <%# Eval("IsMakeTaobao").Equals(0) ?  "δ����" : "�Ѵ���"%>
                          </itemtemplate>
                        </asp:TemplateField>                    
                        <asp:TemplateField HeaderText="����" ItemStyle-Width="19%" HeaderStyle-CssClass=" td_left td_right_fff">
                            <ItemTemplate>
                                <span class="submit_bianji"><a href="<%#"../../API/MakeTaobaoProductData.aspx?productId="+Eval("ProductId")%>" target="_self">�����Ա�����</a></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </UI:Grid>
		  <div class="blank12 clearfix"></div>
      </div>
        <div class="page">
		<div class="bottomPageNumber clearfix">
			<div class="pageNumber">
			<div class="pagination">
				<UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
				</div>
			</div>
		</div>
      </div>
      	</div>
</asp:Content>
