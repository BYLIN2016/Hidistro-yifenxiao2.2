<%@ Page Language="C#" EnableSessionState="True" AutoEventWireup="true" CodeBehind="SubmitTaobaoPurchaseorderCart.aspx.cs"
    Inherits="Hidistro.UI.Web.Shopadmin.purchaseOrder.SubmitTaobaoPurchaseorderCart" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>易分销---采购单</title>
    <Hi:HeadContainer ID="HeadContainer1" runat="server" />
    <Hi:Style ID="Style1" Href="/Shopadmin/css/style.css" runat="server" Media="screen" />
    <Hi:Style ID="Style2" Href="/Shopadmin/css/css.css" runat="server" Media="screen" />
    <Hi:Style ID="Style3" Href="/Shopadmin/css/windows.css" runat="server" Media="screen" />
    <Hi:Style ID="Style4" Href="/Shopadmin/css/pagevalidator.css" runat="server" Media="screen" />
    <Hi:Script ID="Script2" runat="server" Src="/utility/jquery-1.6.4.min.js" />
    <Hi:Script ID="Script3" runat="server" Src="/Shopadmin/script/pagevalidator.js" />
    <Hi:Script ID="Script1" runat="server" Src="/utility/windows.js" />
    <script type="text/javascript">
        function ShowAddDiv(productId)
        {
          $("#serachProductId").val(productId);
          $("#serachResult").empty();
          $("#prevLink").css("display", "none");
          $("#nextLink").css("display", "none");
          $("#pages").text("");
          DivWindowOpen(800,600,'divWindow');         
        }

        function nextPage() {
            var index = $("#currentPage").val()
            index = Number(index) + 1;
            if (index <= 0)
                return;
            $("#currentPage").val(index);
            GetSearchData(index);
        }
        
        function prevPage() {
            var index = $("#currentPage").val()
            index -= 1;
            if (index <= 0) {
                return;
            }
            $("#currentPage").val(index);
            GetSearchData(index);
        }

        function search() {
            $("#currentPage").val(1);
            GetSearchData(1);
        }
        function GetSearchData(pageindex) {
            $.ajax({
                url: "ReturnSearchPurchaseProduct.aspx",
                type: 'post', dataType: 'json', timeout: 10000,
                data: { serachName: $("#serachName").val(), serachSKU: $("#serachSKU").val(), page: pageindex },
                success: function (json2) {
                    var currentPage = $("#currentPage").val();
                    var pageCount = Math.ceil(json2.recCount / 8);
                    if (currentPage >= pageCount)
                        $("#nextLink").css("display", "none");
                    else
                        $("#nextLink").css("display", "");
                    if (currentPage <= 1)
                        $("#prevLink").css("display", "none");
                    else
                        $("#prevLink").css("display", "");

                    $("#pages").text("共" + pageCount + "页 第" + pageindex + "页");


                    $("#serachResult").empty();
                    $("#serachResult").append('<table cellpadding="0" cellspacing="0" width="100%">');
                    $.each(json2.data, function (i, item) {
                        if (item != undefined && item.sku != "") {
                            var str = "<tr><td align='left'><input type='radio'  name='radioSerachResult' value='";
                            str += item.skuId;
                            str += "'/>";
                            str += item.Name;
                            str += "<td>货号：";
                            str += item.sku;
                            str += "</td><td>价格：";
                            str += item.Price;
                            str += "</td><td>库存：";
                            str += item.Stock;
                            str += "</td></tr>";
                            $("#serachResult table").append(str);
                        }
                    });
                    $("#serachResult").append("&nbsp;");
                }
            });
        }
    </script>
</head>
<body>
    <div class="HishipBody">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    淘宝订单合并下单
                </td>
            </tr>
        </table>
    </div>
    <form name="aspnetForm" id="aspnetForm" runat="server">
    <asp:Repeater ID="rpTaobaoOrder" runat="server" OnItemDataBound="rpTaobaoOrder_ItemDataBound">
        <ItemTemplate>
            <div class="taobaoData">
                <table cellpadding="0" cellspacing="0" class="tableShow3">
                    <tr>
                        <td align="left">
                            <strong>
                                <asp:CheckBox Checked="true" ID="chkTbOrder" runat="server" Text='淘宝订单号：' /></strong><span><%# DataBinder.Eval(Container.DataItem, "orderId")%></span>
                        </td>
                        <td width="44%" align="right">
                            <strong>下单时间：</strong><span><%# DataBinder.Eval(Container.DataItem, "createTime")%></span>
                        </td>
                    </tr>
                </table>
                <asp:Repeater ID="reOrderItems" runat="server">
                    <ItemTemplate>
                        <table cellpadding="0" cellspacing="0" class="tableShow2">
                            <tbody>
                                <tr>
                                    <td>
                                        商品图片
                                    </td>
                                    <td>
                                        货号
                                    </td>
                                    <td>
                                        商品名称
                                    </td>
                                    <td>
                                        进货价
                                    </td>
                                    <td>
                                        数量
                                    </td>
                                    <td>
                                        库存
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2">
                                        <Hi:ListImage ID="HiImage2" runat="server" DataField="thumbnailUrl100" />
                                    </td>
                                    <td class="td3">
                                        <div class="fromTaoBao">
                                            <ul class="ul1">
                                            </ul>
                                            <ul class="ul2">
                                                淘宝商品一
                                            </ul>
                                        </div>
                                    </td>
                                    <td class="td1" colspan="5">
                                        <div class="Operating">
                                            <div class="wordInfo">
                                                <strong>
                                                    <%# DataBinder.Eval(Container.DataItem, "title")%></strong>&nbsp;<span>X
                                                        <%# DataBinder.Eval(Container.DataItem, "number")%></span></div>
                                            <div class="submit">
                                                <asp:Button ID="productDel" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id")%>'
                                                    OnCommand="ButtonDelete_Command" runat="server" Text="删除" CssClass="delete" />
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="fromLocal">
                                            <ul class="ul1">
                                            </ul>
                                            <ul class="ul2">
                                                <label runat="server" visible='<%#  ((DataBinder.Eval(Container.DataItem, "localSKU")).ToString())==""?true:false %>'>
                                                    请点击查找选择商品</label>
                                                <%# DataBinder.Eval(Container.DataItem, "localSKU")%>
                                            </ul>
                                        </div>
                                    </td>
                                    <td style="text-align: left">
                                        <%# DataBinder.Eval(Container.DataItem, "localProductName")%><br />
                                        <div class="submit">
                                            <input type="button" onclick='ShowAddDiv("<%# DataBinder.Eval(Container.DataItem, "id")%>",600,500)'
                                                value='<%#  ((DataBinder.Eval(Container.DataItem, "localSKU")).ToString())==""?"查找":"修改" %>'
                                                class="searchbutton" />
                                            <span class="save">
                                                <asp:CheckBox runat="server" Text="保存对应关系" Checked="true" ID="chkSave" Visible='<%#  ((DataBinder.Eval(Container.DataItem, "localSKU")).ToString())==""?false:true %>' />
                                                <asp:Label Style="display: none;" ID="lblSKU" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "localSKU")%>' />
                                                <asp:Label Style="display: none;" ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "title")%>' />
                                                <asp:Label Style="display: none;" ID="lblSpec" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "spec")%>' />
                                            </span>
                                        </div>
                                    </td>
                                    <td title="进货价">
                                        <label runat="server" visible='<%#  ((DataBinder.Eval(Container.DataItem, "localSKU")).ToString())!=""?true:false %>'>
                                            ￥</label><%# DataBinder.Eval(Container.DataItem, "localPrice")%>
                                    </td>
                                    <td>
                                        <div class="control" runat="server" visible='<%#  ((DataBinder.Eval(Container.DataItem, "localSKU")).ToString())!=""?true:false %>'>
                                            <asp:TextBox CssClass="forminput" ID="productNumber" runat="server" ReadOnly="true"
                                                Text='<%# DataBinder.Eval(Container.DataItem, "number")%>' />
                                            <a class="alink1">
                                                <asp:Button ID="productAdd" runat="server" Text="" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id")%>'
                                                    OnCommand="ButtonAdd_Command" /></a> <a class="alink2">
                                                        <asp:Button ID="productDec" runat="server" Text="" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id")%>'
                                                            OnCommand="ButtonDec_Command" /></a>
                                        </div>
                                    </td>
                                    <td class="td2" title="存货">
                                        <%# DataBinder.Eval(Container.DataItem, "localStock")%>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </ItemTemplate>
                </asp:Repeater>
                <table class="tableShow1">
                    <tr>
                        <th align="right">
                            合计：<%# DataBinder.Eval(Container.DataItem, "orderCost")%>
                        </th>
                    </tr>
                </table>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <div class="dataarea mainwidth databody">
        <div class="title title_height m_none td_bottom">
            <em>
                <img height="32" width="32" src="../images/05.gif"></em>
            <h1 class="title_line">
                注意：正确填写收货人地址，以便尽快收到商品！</h1>
        </div>
        <div>
            <div class="datafrom">
                <div class="formitem validator1">
                    <ul>
                        <li><span class="formitemtitle Pw_198">收货人姓名：</span>
                            <asp:TextBox ID="txtShipTo" runat="server" CssClass="forminput"></asp:TextBox>
                            <p runat="server" id="txtShipToTip">
                                姓名长度在20个字符以内</p>
                        </li>
                        <li><span class="formitemtitle Pw_198">收货人地址：</span>
                            <abbr class="formselect">
                                <Hi:RegionSelector runat="server" ID="rsddlRegion" />
                            </abbr>
                        </li>
                        <li><span class="formitemtitle Pw_198">详细地址：</span>
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="forminput"></asp:TextBox>
                            <p runat="server" id="txtAddressTip">
                                详细地址必须控制在100个字符以内</p>
                        </li>
                        <li><span class="formitemtitle Pw_198">邮政编码：</span>
                            <asp:TextBox ID="txtZipcode" runat="server" CssClass="forminput"></asp:TextBox>
                            <p runat="server" id="txtZipcodeTip">
                                邮编长度限制在6-10个字符之间,只能输入数字</p>
                        </li>
                        <li><span class="formitemtitle Pw_198">电话号码：</span>
                            <asp:TextBox ID="txtTel" runat="server" CssClass="forminput"></asp:TextBox>
                            <p runat="server" id="txtTelTip">
                                电话号码长度限制在3-20个字符之间，只能输入数字和字符“-”</p>
                        </li>
                        <li><span class="formitemtitle Pw_198">手机号码：</span>
                            <asp:TextBox ID="txtMobile" runat="server" CssClass="forminput"></asp:TextBox>
                            <p runat="server" id="txtMobileTip">
                                手机号码长度限制在3-20个字符之间,只能输入数字</p>
                        </li>
                        <li><span class="formitemtitle Pw_198">配送方式：</span> <span style="float: left;">
                            <Hi:ShippingModeRadioButtonList ID="radioShippingMode" runat="server" RepeatDirection="Horizontal"
                                RepeatColumns="4" />
                        </span></li>
                        <li class="clear">&nbsp;</li>
                    </ul>
                    <ul class="btntf Pa_198">
                        <asp:Button runat="server" ID="btnSubmit" OnClick="btnSubmit_Click" Text="提交采购单"
                            OnClientClick="return PageIsValid();" CssClass="submit_DAqueding inbnt" />
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <div class="Pop_up" id="divWindow" style="display: none;">
        <h1>
            订单---查找本地商品</h1>
        <div class="img_datala">
            <a onclick="CloseDiv('divWindow')" href="#">
                <img height="20" width="38" src="../images/icon_dalata.gif" style="cursor: pointer;"></a></div>
        <div class="mianform" style="padding-top: 0px;">
            <ul>
                <table width="100%">
                    <tbody>
                        <tr>
                            <td nowrap="nowrap">
                                商品名称：
                            </td>
                            <td>
                                <input id="serachName" class="forminput" />
                            </td>
                            <td nowrap="nowrap">
                                商家编码：
                            </td>
                            <td>
                                <input id="serachSKU" class="forminput" />
                            </td>
                            <td>
                                <input type="button" class="searchbutton" value="查找" onclick="search()" id="btnSearch" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </ul>
            <ul id="serachResult" class="popTable">
            </ul>
            <ul class="up Pa_128 clear">
                <table width="100%" cellspacing="5">
                    <tr>
                        <td>
                            <input type="hidden" name="serachProductId" id="serachProductId" />
                            <asp:Button ID="btnMatch" runat="server" Text="确&nbsp;&nbsp;定" OnClick="btnMatch_Click"
                                CssClass="submit_DAqueding" />
                        </td>
                    </tr>
                </table>
            </ul>
            <ul style="padding-top: 10px; clear: both;">
                <input type="hidden" name="currentPage" id="currentPage" /><span id="pages"></span><a
                    id="prevLink" onclick="prevPage()" href="#">上一页</a> &nbsp;&nbsp;&nbsp;<a id="nextLink"
                        onclick="nextPage()" href="#">下一页</a>
            </ul>
        </div>
    </div>
    </form>
    <script type="text/javascript" language="javascript">
        function InitValidators() {
            initValid(new InputValidator('txtShipTo', 0, 20, false, null, '姓名长度在20个字符以内'));
            initValid(new InputValidator('txtAddress', 0, 100, false, null, '详细地址必须控制在100个字符以内'));
            initValid(new InputValidator('txtTel', 3, 20, true, '^[0-9-]*$', '电话号码长度限制在3-20个字符之间，只能输入数字和字符“-”'));
            initValid(new InputValidator('txtMobile', 3, 20, true, '^[0-9]*$', '手机号码长度限制在3-20个字符之间,只能输入数字'));
            initValid(new InputValidator('txtZipcode', 6, 10, false, '^[0-9]*$', '邮编长度限制在6-10个字符之间,只能输入数字'));
        }

        function ValidationModeName() {
            var reason = document.getElementById("ctl00_contentHolder_radioShippingMode").value;
            if (reason == "undefined") {
                alert("请选择配送方式");
                return false;
            }

            return true;
        }

        $(document).ready(function () {
            InitValidators();
        });
    </script>
</body>
</html>
