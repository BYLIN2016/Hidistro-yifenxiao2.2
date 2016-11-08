
<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ClientSet.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ClientSet" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="blank12 clearfix">
    </div>
    <div class="dataarea mainwidth databody">
        <div class="title m_none td_bottom">
            <em>
                <img src="../images/01.gif" width="32" height="32" /></em>
            <h1>
                客户分组配置
            </h1>
            <span>按照客户的采购时间、采购频次、采购金额等字段进行分组管理多维度筛选条件,
            并可以对这些客户分组进行更加精准的短信关怀和营销。</span>
</div>
        <div class="datafrom">
            <div class="formitem">
          <div class="formitem_con">
              <div>
                  <p>1.如果设置的数字为空或为0，则视为没有设置。   </p>
                  <p>2.设置条件后，系统会自动按照过滤条件给筛选出所匹配的客户。</p>
              </div>
               <h2  >新客户过滤配置 <font color="red">（二选一）</font></h2>
               <div class="webcal" >
                   <div class="radionew2">
                      <input type="radio"  id="radnewtime"  name="radionew" checked="true" runat="server" />　满足注册时间 从 
                      <UI:WebCalendar runat="server"   CssClass="forminput1" ID="calendarStartDate" />
                      
                      至      
                      <UI:WebCalendar runat="server"  CssClass="forminput1" ID="calendarEndDate" />
                     的客户  
                  </div>
                  <div class="radionew2" style="margin-top:15px;"><input type="radio" id="radnewday" name="radionew" runat="server" />　满足
                     <samp class="formselect"> <select name="slsnewregist" id="slsnewregist" runat="server">
                          <option value="3">前3天</option>
                          <option value="7">前7天</option>
                          <option value="14">前14天</option>
                      </select>　时间内注册的客户
                      </samp>
                   </div>   
             </div>
             <h2 >活跃客户过滤配置 <font color="red">（二选一）</font></h2> 
             <div class="webcal">
                 <div class="radionew2">
                     <input id="radioactivyorder" name="radioactivy" type="radio" checked="true" runat="server"/>　满足下单次数  
                    <samp class="formselect"><select id="slsactivyorder" name="slsactivyorder" runat="server">
                         <option value="7">前7天</option>
                         <option value="14">前14天</option>
                         <option value="30">前30天</option>
                     </select>
                     </samp>
                      内采购
                      <samp class="formselect">
                     <select name="slsactivyorderchar" id="slsactivyorderchar" runat="server">
                         <option value=">">大于</option>
                         <option value=">=">大于等于</option>
                     </select>
                     </samp>
                     <input type="text" id="txtactivyorder" runat="server" Class="forminput" style="float:none" />　次的客户
                 </div>
                 <div class="radionew2" style="margin-top:15px;">
                      <input id="radioactivymoney" name="radioactivy" type="radio" runat="server"/>　满足采购金额 
                      <samp class="formselect">  
                     <select name="slsactivymoney" id="slsactivymoney" runat="server">
                         <option value="7">前7天</option>
                         <option value="14">前14天</option>
                         <option value="30">前30天</option>
                     </select>
                     </samp>
                      内采购
                      <samp class="formselect">
                     <select id="slsactivymoneychar" runat="server">
                         <option value=">">大于</option>
                         <option value=">=">大于等于</option>
                     </select>
                     </samp>
                     <input type="text" id="txtactivymoney" runat="server" Class="forminput" style="float:none" />　元的客户
                 </div>
             </div>
             <h2 >休眠客户配置 </h2> 
             <div class="webcal">
               　满足前　
               <samp class="formselect">
               <select name="slssleep" id="slssleep" runat="server">
                         <option value="7">前7天</option>
                         <option value="14">前14天</option>
                         <option value="30">前30天</option>
                     </select>  
                     </samp>
                     内无采购记录的客户
             </div>
          </div>  
          <ul class="btntf Pa_140" style="margin-top:15px;">
		     <asp:Button ID="btnSaveClientSettings" runat="server" Text="保 存" CssClass="submit_DAqueding float" OnClientClick="return validForm()" />
      </ul>
</div>

        </div>
    </div>
    <div class="bottomarea testArea">
        <!--顶部logo区域-->
    </div>
       <script>
           function validForm() {
                var reg1 =  /^\d+$/;
                var re = /^\d+(?=\.{0,1}\d+$|$)/
                if ($("#ctl00_contentHolder_txtactivyorder").val().replace(/\s/g, "")!= "") {
                    if (!reg1.test($("#ctl00_contentHolder_txtactivyorder").val().replace(/\s/g, ""))) {
                        alert("下单次数非法");
                        return false;
                    }
                }

                if ($("#ctl00_contentHolder_txtactivymoney").val().replace(/\s/g, "") != "") {
                    if (!re.test($("#ctl00_contentHolder_txtactivymoney").val())) {
                        alert("下单金额非法");
                        return false;
                    }
                }
           }
       </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>