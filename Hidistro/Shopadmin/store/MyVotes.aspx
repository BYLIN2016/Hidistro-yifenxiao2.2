<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="MyVotes.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.MyVotes" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="optiongroup mainwidth">   
      
      <ul>
        <li class="optionstar"><a href="MyFriendlyLinks.aspx"><span>友情链接</span></a></li>
        <li><a href="ManageMyHotKeywords.aspx" class="optionnext"><span>热门关键字</span></a></li>
        <li class="menucurrent"><a href="#" class="optioncurrentend"><span class="optioncenter">投票调查</span></a></li>
      </ul>
</div>
  <div class="blank12 clearfix"></div>
  <div class="dataarea mainwidth databody">
    <div class="title"> <em><img src="../images/01.gif" width="32" height="32" /></em>
      <h1>投票调查管理 </h1>
     <span>管理店铺的在线投票调查，您可以新建投票或查看原有投票数据。这里允许创建多个投票调查，但只能设定一个在前台显示</span></div>
    <!-- 添加按钮-->
    <div class="btn">
      <a href="AddMyVote.aspx" class="submit_jia">添加新投票</a>
    </div>
    <div class="datalist">
    <asp:DataList ID="dlstVote" runat="server" Width="100%" DataKeyField="VoteId">
                <HeaderTemplate>
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr class="table_title">
                      <th width="5%" class=" td_right td_left">展开 </td>
                      <th width="30%" class=" td_right td_left">投票标题 </td>
                      <th width="11%" class=" td_right td_left">最多可选项数 </td>
                      <th width="9%" class=" td_right td_left">投票总数 </td>
                      <th width="13%" class=" td_right td_left">在前台是否显示 </td>
                      <th width="22%" colspan="2" class=" td_left td_right_fff">操作 </td>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
              <tr>
                <td><img id="themesImg" src="../../utility/pics/plus.gif" onclick="errorEventTable(this);" /></td>
                <td>
                    <asp:Label ID="lblVoteName" runat="server" Text='<%# Eval("VoteName") %>'></asp:Label>
                </td>
                <td><asp:Label ID="lblMaxCheck" runat="server" Text='<%# Eval("MaxCheck") %>' ></asp:Label></td>
                <td><asp:Label ID="lblVoteCounts" runat="server" Text='<%# Eval("VoteCounts") %>' ></asp:Label></td>
                <td>
                    <asp:ImageButton ID="lmgbtnIsBackup" runat="server" CommandName="IsBackup" ImageUrl='<%# (bool)Eval("IsBackup") ? "../../utility/pics/true.gif" : "../../utility/pics/false.gif" %>'></asp:ImageButton>
                </td>
                <td colspan="2">
                   <span class="submit_bianji"><a href='<%# "EditMyVote.aspx?VoteId=" + Eval("VoteId")%> '>编辑</a></span>
                   <span class="submit_shanchu"><Hi:ImageLinkButton ID="lkbDelete"  runat="server"  CommandName="Delete" Text="删除" IsShow="true" /></span>
                </td>
              </tr>
              <tr style="display: none;" >
                <td colspan="8">
                  <div class="tpiao">                  
                    <asp:GridView ID="grdVoteItem" runat="server" AutoGenerateColumns="false" ShowHeader="true" DataKeyNames="VoteItemId" GridLines="None" HeaderStyle-CssClass="table_title" Width="76%">                                                        
                    <Columns>                                       
		                <asp:TemplateField HeaderText="选项值" ControlStyle-Width="50px" HeaderStyle-CssClass="spanB">
		                    <ItemTemplate>
		                        <asp:Label ID="lblVoteItemName" runat="server" Text='<%# Eval("VoteItemName") %>'></asp:Label>
		                    </ItemTemplate>
		                    <EditItemTemplate>
		                        <Hi:HtmlDecodeTextBox ID="txtVoteItemName" runat="server" Text='<%# Eval("VoteItemName") %>' />
		                    </EditItemTemplate>
		                </asp:TemplateField>
		                <asp:TemplateField HeaderText="比例示意图" ControlStyle-Width="150px" HeaderStyle-CssClass="spanB">
                            <ItemTemplate>
                              <div style='width:<%# string.Format("{0}px", Eval("Lenth")) %>'  class="votelenth"/></div>
                                <img width='<%# string.Format("{0}px", Eval("Lenth")) %>' height="15" class="spanE"/>
                            </ItemTemplate>
                        </asp:TemplateField>    
                        <asp:TemplateField HeaderText="百分比" ControlStyle-Width="50px" HeaderStyle-CssClass="spanB">
                            <ItemTemplate>
                                <asp:Label ID="lblPercentage" runat="server" Text='<%# string.Format("{0}%", Eval("Percentage")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>  
                        <asp:BoundField HeaderText="票数" DataField="ItemCount"  ReadOnly="true" ControlStyle-Width="50px" HeaderStyle-CssClass="spanB" />
                    </Columns>
                    </asp:GridView>
                  </div>
                </td>
              </tr>
            </ItemTemplate>           
            
            <FooterTemplate>
                </table>
            </FooterTemplate>
          </asp:DataList>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    var isIE = !!document.all;
    function errorEventTable(tableObject) {
        tableObject.runat = "server";
        if (isIE) {
            var nextNodeObject = tableObject.parentNode.parentNode.nextSibling;
        }
        else {
            var nextNodeObject = tableObject.parentNode.parentNode.nextSibling.nextSibling;
        }
        (nextNodeObject.style.display == "none") ? nextNodeObject.style.display = "" : nextNodeObject.style.display = "none";
        (nextNodeObject.style.display == "none") ? tableObject.src = '<%= Globals.ApplicationPath + "/utility/pics/plus.gif" %>' : tableObject.src = '<%= Globals.ApplicationPath + "/utility/pics/minus.gif" %>';
    }

</script>
</asp:Content>
