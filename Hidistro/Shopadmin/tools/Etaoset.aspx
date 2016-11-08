<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Etaoset.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.Etaoset"
MasterPageFile="~/Shopadmin/ShopAdmin.Master"  %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
    <div class="dataarea mainwidth databody">
        <div class="title m_none m_none td_bottom">
            <em>
                <img src="../images/01.gif" width="32" height="32" /></em>
            <h1>
                ����һ�ԣ�www.etao.com����ƷFEED����</h1>
            <span>��������������̳���פһ�ԣ�open.etao.com��,��ͨ����������ý������̳ǵ���Ʒ��������Ϣ����Feed�ļ����ύ��һ�ԣ�open.etao.com����¼��Ʒ�ͷ�����Ϣ��</span>
        </div>
        <div>   
        <div align="center" style="width:90%"> <asp:Button ID="BtnCreateEtao"  
                Visible="false"  CssClass="submit_DAqueding "  runat="server" Text="���뿪ͨһ��" 
                onclick="BtnCreateEtao_Click" />&nbsp;&nbsp;&nbsp; <asp:Label
            ID="lbEtaoCreate" runat="server" Text=""  ForeColor="Red"  Visible="false" ></asp:Label></div>
        <div class="datafrom"  id="etaoset" runat="server" visible="false">  
       
            <div class="formitem" >
                <ul>
                    <li><span>���ϴ�һ��TXT��֤�ļ���</span>
                        <asp:FileUpload ID="fudVerifyFile" runat="server" CssClass="forminput" />
                        &nbsp;<asp:Button ID="btnUpoad" runat="server" Text="�ϴ�" CssClass="submit_queding" OnClick="btnUpoad_Click" />
                        <p id="ctfile">
                            ���ϴ��̼���һ�ԣ�etao.com����վ������פʱ���������������֤ʱ���ص���֤�ļ���</p>
                    </li>
                    <li><span>������һ���ʺţ�<em>*</em></span>
                        <asp:TextBox ID="txtEtaoID" runat="server" CssClass="forminput" />
                        <p id="txtEtaoIDTip">
                            ����,�̼�������פһ�ԣ�etao.com����վʱʹ�õ��˺ţ�1-60���ַ�</p>
                    </li>
                    <li><span>�Ƿ���:</span>
                        <Hi:YesNoRadioButtonList ID="rdobltIsCreateFeed" SelectedValue="false" RepeatLayout="Flow"
                            runat="server" />
                            <p>ѡ���Ƿ������ɹ�һ�ԣ�Etao.com�����������Զ�ץȡ����ƷFEED�ļ���Ĭ�ϲ�������</p>
                    </li>
                    <li runat="server"  id="incDir"><span>��������Ŀ¼:</span><asp:Label runat="server" ID="lblEtaoFeedInc" />
                      <p>��һ�ԣ�Etao.com�����������Զ�ץȡ��������ƷFEED�ļ��Ĵ��Ŀ¼��ÿ��30���ӻ�ץȡһ�Ρ�</p>
   
                    </li>
                    <li runat="server"  id="fulDir"><span>ȫ������Ŀ¼:</span><asp:Label runat="server" ID="lblEtaoFeedFull" />
                                   <p>��һ�ԣ�Etao.com�����������Զ�ץȡ��ȫ����ƷFEED�ļ��Ĵ��Ŀ¼��ÿ���賿��ץȡһ�Ρ�</p>
                                   </li>
                </ul>
                <div style="clear: both">
                </div>
                <ul class="btntf Pa_140" style="margin: 5px 0px; height: 30px;">
                    <asp:Button ID="btnChangeEmailSettings" runat="server" Text="�� ��" 
                        CssClass="submit_DAqueding float" onclick="btnChangeEmailSettings_Click">
                    </asp:Button>
                </ul>
            </div>
        </div>
    </div>
    </div>
</asp:Content>
