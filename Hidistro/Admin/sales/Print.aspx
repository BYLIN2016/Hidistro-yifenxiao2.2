<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Print.aspx.cs" Inherits="Hidistro.UI.Web.Admin.sales.Print" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        /**
 *    打印相关
*/
        @media print
        {
            .notprint
            {
                display: none;
            }
            .PageNext
            {
                page-break-after: always;
            }
        }
        @media screen
        {
            .notprint
            {
                display: inline;
                cursor: hand;
            }
        }
        .submit_DAqueding{ width:100px; height:38px;background:url(../images/icon.gif) no-repeat -159px -362px; color:#FFF; border:0;font-size:14px; font-weight:700;cursor:pointer;}
    </style>
</head>
<body style="text-align: center;margin:0 auto;">
<script language="javascript" src="../js/LodopFuncs.js"></script>
<object  id="LODOP_OB" classid="clsid:2105C259-1E0C-4534-8141-A753534CB4CA" width=0 height=0> 
       <embed id="LODOP_EM" type="application/x-print-lodop" width=0 height=0></embed>
</object>
    <script type="text/javascript">
        var LODOP = getLodop(document.getElementById('LODOP_OB'), document.getElementById('LODOP_EM'));
        function clicks() {
            try{
            var w = eval(<%=width %>*10);
            var h = eval(<%=height %>*10);
            LODOP.PRINT_INIT("打印快递单");
            LODOP.SET_PRINT_PAGESIZE(3,w,h,"");
            LODOP.ADD_PRINT_HTM(0, 0, w, h, document.getElementById("divContent").innerHTML);
            LODOP.PRINTA();           
            }catch(e)
            {
             alert("请先安装打印控件！");
             return false;
            }
            document.printForm.submit();
        }
    </script>
    <div style="text-align: center;margin:0 auto;" id="divContent" runat="server">
        
    </div>
    <br />
    <input type="button" value="确认打印" class="notprint submit_DAqueding" id="printBtn" onclick="clicks()"/>
    <form action="PrintComplete.aspx" name="printForm" method="post">
        <input type="hidden" name="orderIds" value="<%= orderIds %>" />
        <input type="hidden" name="mailNo" value="<%= mailNo %>" />
        <input type="hidden" name="templateName" value="<%= templateName %>" />
        </form>
        
</body>
</html>
