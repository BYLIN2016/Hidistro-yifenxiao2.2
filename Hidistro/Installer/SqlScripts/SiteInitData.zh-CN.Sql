-- 分销商等级
DELETE FROM aspnet_DistributorGrades
-- 会员等级
DELETE FROM aspnet_MemberGrades

-- 信息模板
DELETE FROM Hishop_MessageTemplates

-- 友情链接
DELETE FROM Hishop_FriendlyLinks

-- 分销商等级
INSERT INTO aspnet_DistributorGrades([Name], [Description], [Discount]) VALUES(N'金牌代理', N'金牌代理', 80)
INSERT INTO aspnet_DistributorGrades([Name], [Description], [Discount]) VALUES(N'银牌代理', N'银牌代理', 90)
INSERT INTO aspnet_DistributorGrades([Name], [Description], [Discount]) VALUES(N'铜牌代理', N'铜牌代理', 100)

INSERT INTO aspnet_MemberGrades([Name], [Description], [Points], [IsDefault], [Discount]) VALUES(N'普通会员', N'普通会员', 0, 1, 100)
INSERT INTO aspnet_MemberGrades([Name], [Description], [Points], [IsDefault], [Discount]) VALUES(N'高级会员', N'高级会员', 1000, 0, 90)
INSERT INTO aspnet_MemberGrades([Name], [Description], [Points], [IsDefault], [Discount]) VALUES(N'VIP会员', N'VIP会员', 5000, 0, 80)

--信息模板
/********************************************************************************************************************************************************************/ 

/****** Object:  Table [dbo].[Hishop_MessageTemplates]    Script Date: 05/26/2010 16:08:59 ******/
GO
INSERT [dbo].[Hishop_MessageTemplates] ([MessageType], [Name], [SendEmail], [SendSMS], [SendInnerMessage], [TagDescription], [EmailSubject], [EmailBody], [InnerMessageSubject], [InnerMessageBody], [SMSBody]) VALUES (N'ChangedDealPassword', N'会员修改交易密码时', 0, 0, 0, N'<TABLE border=0 cellSpacing=0 cellPadding=2 width="600" align=left>
        <TBODY>
          <TR>
            <TD width="33%">用户名：$Username$</TD>
            <TD width="33%">交易密码：$DealPassword$</TD>
            <TD>店铺名称：$SiteName$</TD>
          </TR>
        </TBODY>
      </TABLE>
', N'您的交易密码已经修改', N'<table border="0" cellspacing="1" cellpadding="0" style="border-bottom: #336699 1px solid; border-left: #336699 1px solid; border-top: #336699 1px solid; border-right: #336699 1px solid">
    <tbody>
        <tr>
            <td style="border-bottom: #336699 1px solid">
            <table border="0" cellspacing="0" cellpadding="10" width="580">
                <tbody>
                    <tr>
                        <td valign="top" width="261">
                        <p><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2"><strong>Dear $Username$</strong> ，<br />
                        <br />
                        您好！您的交易密码已经修改，交易密码是保护您预付款账户安全的重要手段，请您妥善保管。</font></font></p>
                        <strong>$SiteName$</strong></td>
                        <td valign="top" width="279">
                        <table border="0" cellspacing="0" cellpadding="3" width="100%" bgcolor="#ffffff" align="center">
                            <tbody>
                                <tr>
                                    <td bgcolor="#e1edfe" align="center"><font color="#003366" size="2" face="Verdana, Arial, Helvetica, sans-serif"><b>您的新交易密码</b></font></td>
                                </tr>
                                <tr>
                                    <td bgcolor="#ffffee">
                                    <table border="0" cellspacing="0" cellpadding="2" width="100%" align="left">
                                        <tbody>
                                            <tr>
                                                <td class="small" width="35%">
                                                <div align="right"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2"><strong>用户名：</strong> </font></font></div>
                                                </td>
                                                <td class="small" width="65%"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2">$Username$</font></font></td>
                                            </tr>
                                            <tr>
                                                <td class="small">
                                                <div align="right"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2"><strong>新交易密码：</strong></font></font></div>
                                                </td>
                                                <td class="small"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2">$DealPassword$</font></font></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td bgcolor="#ffffee">&nbsp;</td>
                                </tr>
                            </tbody>
                        </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            </td>
        </tr>
        <tr>
            <td bgcolor="#ffffff">
            <table border="0" cellspacing="0" cellpadding="10">
                <tbody>
                    <tr>
                        <td valign="top" width="396"><font size="2" face="Verdana, Arial, Helvetica, sans-serif">■重要信息：此邮件包含个人机密信息，请妥善保存！</font></td>
                    </tr>
                </tbody>
            </table>
            </td>
        </tr>
    </tbody>
</table>', N'您的交易密码已经修改', N'您已经修改了交易密码，新的交易密码是：$DealPassword$，请您妥善保管好个人资料。', N'您已经修改了交易密码，新的交易密码是：$DealPassword$，请您妥善保管好个人资料。')
INSERT [dbo].[Hishop_MessageTemplates] ([MessageType], [Name], [SendEmail], [SendSMS], [SendInnerMessage], [TagDescription], [EmailSubject], [EmailBody], [InnerMessageSubject], [InnerMessageBody], [SMSBody]) VALUES (N'ChangedPassword', N'会员修改登录密码时', 0, 0, 0, N'<TABLE border=0 cellSpacing=0 cellPadding=2 width="600" align=left>
        <TBODY>
          <TR>
            <TD width="33%">用户名：$Username$</TD>
            <TD width="33%">登录密码：$Password$</TD>
            <TD>店铺名称：$SiteName$</TD>
          </TR>
        </TBODY>
      </TABLE>', N'您的登录密码已经修改', N'<table border="0" cellspacing="1" cellpadding="0" style="border-bottom: #336699 1px solid; border-left: #336699 1px solid; border-top: #336699 1px solid; border-right: #336699 1px solid">
    <tbody>
        <tr>
            <td style="border-bottom: #336699 1px solid">
            <table border="0" cellspacing="0" cellpadding="10" width="580">
                <tbody>
                    <tr>
                        <td valign="top" width="261">
                        <p><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2"><strong>Dear $Username$</strong> ，<br />
                        <br />
                        您好！您的登录密码已经修改，您在下次登录时需要使用新的登录密码，您还可以随时进入会员中心查看或修改您的其他个人资料。</font></font></p>
                        <strong>$SiteName$</strong></td>
                        <td valign="top" width="279">
                        <table border="0" cellspacing="0" cellpadding="3" width="100%" bgcolor="#ffffff" align="center">
                            <tbody>
                                <tr>
                                    <td bgcolor="#e1edfe" align="center"><font color="#003366" size="2" face="Verdana, Arial, Helvetica, sans-serif"><b>您的新密码</b></font></td>
                                </tr>
                                <tr>
                                    <td bgcolor="#ffffee">
                                    <table border="0" cellspacing="0" cellpadding="2" width="100%" align="left">
                                        <tbody>
                                            <tr>
                                                <td class="small" width="29%">
                                                <div align="right"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2"><strong>用户名：</strong> </font></font></div>
                                                </td>
                                                <td class="small" width="71%"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2">$Username$</font></font></td>
                                            </tr>
                                            <tr>
                                                <td class="small">
                                                <div align="right"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2"><strong>新密码：</strong></font></font></div>
                                                </td>
                                                <td class="small"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2">$Password$</font></font></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td bgcolor="#ffffee">&nbsp;</td>
                                </tr>
                            </tbody>
                        </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            </td>
        </tr>
        <tr>
            <td bgcolor="#ffffff">
            <table border="0" cellspacing="0" cellpadding="10">
                <tbody>
                    <tr>
                        <td valign="top" width="396"><font size="2" face="Verdana, Arial, Helvetica, sans-serif">■重要信息：此邮件包含个人机密信息，请妥善保存！</font></td>
                    </tr>
                </tbody>
            </table>
            </td>
        </tr>
    </tbody>
</table>', N'您的登录密码已经修改', N'您已经修改了登录密码，新的登录密码是：$Password$，请您妥善保管好个人资料。', N'您已经修改了登录密码，新的登录密码是：$Password$，请您妥善保管好个人资料。')
INSERT [dbo].[Hishop_MessageTemplates] ([MessageType], [Name], [SendEmail], [SendSMS], [SendInnerMessage], [TagDescription], [EmailSubject], [EmailBody], [InnerMessageSubject], [InnerMessageBody], [SMSBody]) VALUES (N'ForgottenPassword', N'会员找回登录密码', 0, 0, 0, N'<TABLE border=0 cellSpacing=0 cellPadding=2 width="600" align=left>
        <TBODY>
          <TR>
            <TD width="33%">用户名：$Username$</TD>
            <TD width="33%">登录密码：$Password$</TD>
            <TD>店铺名称：$SiteName$</TD>
          </TR>
        </TBODY>
      </TABLE>', N'系统为您生成了新的登录密码', N'<table border="0" cellspacing="1" cellpadding="0" style="border-bottom: #336699 1px solid; border-left: #336699 1px solid; border-top: #336699 1px solid; border-right: #336699 1px solid">
    <tbody>
        <tr>
            <td style="border-bottom: #336699 1px solid">
            <table border="0" cellspacing="0" cellpadding="10" width="580">
                <tbody>
                    <tr>
                        <td valign="top" width="261">
                        <p><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2"><strong>Dear $Username$</strong> ，<br />
                        <br />
                        您好！您通过系统找回了登录密码，为了安全起见，系统为您随机生成了一个新的登录密码，请您在看到新密码以后尽快登录会员中心重新设置您的登录密码。</font></font></p>
                        <strong>$SiteName$</strong></td>
                        <td valign="top" width="279">
                        <table border="0" cellspacing="0" cellpadding="3" width="100%" bgcolor="#ffffff" align="center">
                            <tbody>
                                <tr>
                                    <td bgcolor="#e1edfe" align="center"><font color="#003366" size="2" face="Verdana, Arial, Helvetica, sans-serif"><b>找回密码</b></font></td>
                                </tr>
                                <tr>
                                    <td bgcolor="#ffffee">
                                    <table border="0" cellspacing="0" cellpadding="2" width="100%" align="left">
                                        <tbody>
                                            <tr>
                                                <td class="small" width="35%">
                                                <div align="right"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2"><strong>用户名：</strong> </font></font></div>
                                                </td>
                                                <td class="small" width="65%"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2">$Username$</font></font></td>
                                            </tr>
                                            <tr>
                                                <td class="small">
                                                <div align="right"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2"><strong>新密码：</strong></font></font></div>
                                                </td>
                                                <td class="small"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2">$Password$</font></font></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td bgcolor="#ffffee">&nbsp;</td>
                                </tr>
                            </tbody>
                        </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            </td>
        </tr>
        <tr>
            <td bgcolor="#ffffff">
            <table border="0" cellspacing="0" cellpadding="10">
                <tbody>
                    <tr>
                        <td valign="top" width="396"><font size="2" face="Verdana, Arial, Helvetica, sans-serif">■重要信息：此邮件包含个人机密信息，请妥善保存！</font></td>
                    </tr>
                </tbody>
            </table>
            </td>
        </tr>
    </tbody>
</table>', N'系统为您生成了新的登录密码', N'您忘记登录密码了吗？密码已经修改，新登录密码为：$Password$。建议您尽快重新设置登录密码。', N'您忘记登录密码了吗？密码已经修改，新登录密码为：$Password$。建议您尽快重新设置登录密码。')
INSERT [dbo].[Hishop_MessageTemplates] ([MessageType], [Name], [SendEmail], [SendSMS], [SendInnerMessage], [TagDescription], [EmailSubject], [EmailBody], [InnerMessageSubject], [InnerMessageBody], [SMSBody]) VALUES (N'NewUserAccountCreated', N'会员注册时', 1, 0, 0, N'<TABLE border=0 cellSpacing=0 cellPadding=2 width="600" align=left>
        <TBODY>
          <TR>
            <TD width="33%">用户名：$Username$</TD>
            <TD width="33%">邮箱：$Email$</TD>
            <TD>登录密码：$Password$</TD>
          </TR>
          <TR>
            <TD>店铺名称：$SiteName$</TD>
            <TD>&nbsp;</TD>
            <TD>&nbsp;</TD>
          </TR>
          <TR>
            <TD colspan="3">&nbsp;</TD>
          </TR>
        </TBODY>
      </TABLE>', N'欢迎您的到来', N'<table border="0" cellspacing="1" cellpadding="0" style="border-bottom: #336699 1px solid; border-left: #336699 1px solid; border-top: #336699 1px solid; border-right: #336699 1px solid">
    <tbody>
        <tr>
            <td style="border-bottom: #336699 1px solid">
            <table border="0" cellspacing="0" cellpadding="10" width="580">
                <tbody>
                    <tr>
                        <td valign="top" width="261">
                        <p><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2"><strong>Dear $Username$</strong> ，<br />
                        <br />
                        欢迎您的到来！</font><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2">您可以随时进入会员中心查看或修改您的个人资料</font></font><font size="2">。祝您购物愉快！</font></font></p>
                        <strong>$SiteName$</strong></td>
                        <td valign="top" width="279">
                        <table border="0" cellspacing="0" cellpadding="3" width="100%" bgcolor="#ffffff" align="center">
                            <tbody>
                                <tr>
                                    <td bgcolor="#e1edfe" align="center"><font color="#003366" size="2" face="Verdana, Arial, Helvetica, sans-serif"><b>注册信息</b></font></td>
                                </tr>
                                <tr>
                                    <td bgcolor="#ffffee">
                                    <table border="0" cellspacing="0" cellpadding="2" width="100%" align="left">
                                        <tbody>
                                            <tr>
                                                <td class="small" width="31%">
                                                <div align="right"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2"><strong>用户名：</strong> </font></font></div>
                                                </td>
                                                <td class="small" width="69%"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2">$Username$</font></font></td>
                                            </tr>
                                            <tr>
                                                <td class="small">
                                                <div align="right"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2"><strong>登录密码：</strong></font></font></div>
                                                </td>
                                                <td class="small"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2">$Password$</font></font></td>
                                            </tr>
                                            <tr>
                                                <td class="small">
                                                <div align="right"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2"><strong>电子邮箱：</strong></font></font></div>
                                                </td>
                                                <td class="small"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2">$Email$</font></font></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td bgcolor="#ffffee">&nbsp;</td>
                                </tr>
                            </tbody>
                        </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            </td>
        </tr>
        <tr>
            <td bgcolor="#ffffff">
            <table border="0" cellspacing="0" cellpadding="10">
                <tbody>
                    <tr>
                        <td valign="top" width="396"><font size="2" face="Verdana, Arial, Helvetica, sans-serif">■重要信息：此邮件包含个人机密信息，请妥善保存！</font></td>
                    </tr>
                </tbody>
            </table>
            </td>
        </tr>
    </tbody>
</table>', N'欢迎您的到来', N'欢迎您的到来，您的用户名：$Username$，登录密码：$Password$，电子邮件：$Email$。请妥善保管好您的注册信息。', N'欢迎您的到来，您的用户名：$Username$，登录密码：$Password$，电子邮件：$Email$。请妥善保管好您的注册信息。')
INSERT [dbo].[Hishop_MessageTemplates] ([MessageType], [Name], [SendEmail], [SendSMS], [SendInnerMessage], [TagDescription], [EmailSubject], [EmailBody], [InnerMessageSubject], [InnerMessageBody], [SMSBody]) VALUES (N'OrderClosed', N'订单关闭以后', 0, 0, 0, N'<TABLE border=0 cellSpacing=0 cellPadding=2 width="650" align=left>
        <TBODY>
          <TR>
            <TD width="33%">用户名：$Username$</TD>
            <TD width="33%">订单号：$OrderId$</TD>
            <TD>关闭原因：$CloseReason$</TD>
          </TR>
          <TR>
            <TD>店铺名称：$SiteName$</TD>
            <TD>&nbsp;</TD>
            <TD>&nbsp;</TD>
          </TR>
        </TBODY>
      </TABLE>', N'订单已关闭', N'<table border="0" cellspacing="1" cellpadding="0" style="border-bottom: #336699 1px solid; border-left: #336699 1px solid; border-top: #336699 1px solid; border-right: #336699 1px solid">
    <tbody>
        <tr>
            <td style="border-bottom: #336699 1px solid">
            <table border="0" cellspacing="0" cellpadding="10" width="580">
                <tbody>
                    <tr>
                        <td valign="top" width="261">
                        <p><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2"><strong>Dear $Username$</strong>&nbsp;，<br />
                        <br />
                        您好！您的订单已经关闭，请查看右侧相关信息，欢迎您继续选购其他商品，祝您购物愉快！</font></font></p>
                        <strong>$SiteName$</strong></td>
                        <td valign="top" width="279">
                        <table border="0" cellspacing="0" cellpadding="3" width="100%" bgcolor="#ffffff" align="center">
                            <tbody>
                                <tr>
                                    <td bgcolor="#e1edfe" align="center"><font color="#003366" size="2" face="Verdana, Arial, Helvetica, sans-serif"><b>订单信息</b></font></td>
                                </tr>
                                <tr>
                                    <td bgcolor="#ffffee">
                                    <table border="0" cellspacing="0" cellpadding="2" width="100%" align="left">
                                        <tbody>
                                            <tr>
                                                <td class="small" width="29%">
                                                <div align="right"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2"><strong>订单号：</strong> </font></font></div>
                                                </td>
                                                <td class="small" width="71%"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2">$OrderId$</font></font></td>
                                            </tr>
                                            <tr>
                                                <td class="small">
                                                <div align="right"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2"><strong>关闭原因：</strong></font></font></div>
                                                </td>
                                                <td class="small"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2">$CloseReason$</font></font></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td bgcolor="#ffffee">&nbsp;</td>
                                </tr>
                            </tbody>
                        </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            </td>
        </tr>
        <tr>
            <td bgcolor="#ffffff">
            <table border="0" cellspacing="0" cellpadding="10">
                <tbody>
                    <tr>
                        <td valign="top" width="396"><font size="2" face="Verdana, Arial, Helvetica, sans-serif">■重要信息，请妥善保存！</font></td>
                    </tr>
                </tbody>
            </table>
            </td>
        </tr>
    </tbody>
</table>', N'订单已关闭', N'您的订单已经关闭，欢迎您继续选购其他商品。订单号：$OrderId$，关闭原因：$CloseReason$。', N'您的订单已经关闭，欢迎您继续选购其他商品。订单号：$OrderId$，关闭原因：$CloseReason$。')
INSERT [dbo].[Hishop_MessageTemplates] ([MessageType], [Name], [SendEmail], [SendSMS], [SendInnerMessage], [TagDescription], [EmailSubject], [EmailBody], [InnerMessageSubject], [InnerMessageBody], [SMSBody]) VALUES (N'OrderCreated', N'订单创建时', 0, 0, 0, N'<TABLE border=0 cellSpacing=0 cellPadding=2 width="650" align=left><TBODY><TR><TD width="33%">用户名：$Username$</TD><TD width="33%">订单号：$OrderId$</TD><TD>订单金额：$Total$</TD></TR><TR><TD>备注：$Memo$</TD><TD>配送方式：$Shipping_Type$</TD><TD>收货人姓名：$Shipping_Name$</TD></TR><TR><TD>收货人地址：$Shipping_Addr$</TD><TD>收货人邮编：$Shipping_Zip$</TD><TD>收货人电话：$Shipping_Phone$</TD></TR><TR><TD>收货人手机：$Shipping_Cell$</TD><TD>收货人Email：$Shipping_Email$</TD><TD>店铺名称：$SiteName$</TD></TR></TBODY></TABLE>', N'订单已提交成功', N'<table border="0" cellspacing="1" cellpadding="0" style="border-bottom: #336699 1px solid; border-left: #336699 1px solid; border-top: #336699 1px solid; border-right: #336699 1px solid">
    <tbody>
        <tr>
            <td style="border-bottom: #336699 1px solid">
            <table border="0" cellspacing="0" cellpadding="10" width="580">
                <tbody>
                    <tr>
                        <td valign="top">
                        <p><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2"><strong>Dear $Username$</strong>&nbsp;，<br />
                        <br />
                        您好！感谢您的订购，您的订单已经生成，请查看以下信息：<br />
                        如果您要查看或修改您的订单，请登录进入会员中心，在订单管理页面中进行相关操作。 <br />
                        </font></font></p>
                        <table border="0" cellspacing="0" cellpadding="3" width="100%" bgcolor="#ffffff" align="center">
                            <tbody>
                                <tr>
                                    <td bgcolor="#e1edfe" align="center"><font color="#003366" size="2" face="Verdana, Arial, Helvetica, sans-serif"><b>订单信息</b></font></td>
                                </tr>
                                <tr>
                                    <td bgcolor="#ffffee">
                                    <table border="0" cellspacing="0" cellpadding="2" width="100%" align="left">
                                        <tbody>
                                            <tr>
                                                <td class="small" width="15%">
                                                <div align="right"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2"><strong>订单号：</strong> </font></font></div>
                                                </td>
                                                <td class="small" width="85%"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2">$OrderId$</font></font></td>
                                            </tr>
                                            <tr>
                                                <td class="small">
                                                <div align="right"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2"><strong>总金额：</strong></font></font></div>
                                                </td>
                                                <td class="small"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2">$Total$</font></font></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                <div align="right"><font size="2"><strong>收货人：</strong></font></div>
                                                </td>
                                                <td><font size="2" face="Verdana, Arial, Helvetica, sans-serif">$Shipping_Name$</font></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                <div align="right"><font size="2"><strong>送货地址：</strong></font></div>
                                                </td>
                                                <td><font size="2" face="Verdana, Arial, Helvetica, sans-serif">$Shipping_Addr$</font></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                <div align="right"><font size="2"><strong>备注：</strong></font></div>
                                                </td>
                                                <td><font size="2" face="Verdana, Arial, Helvetica, sans-serif">$Memo$</font></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td bgcolor="#ffffee">&nbsp;</td>
                                </tr>
                            </tbody>
                        </table>
                        <br />
                        <font size="2">此订单确认信仅确认我们已经收到了您的订单<br />
                        欢迎您在遇到问题的时候与我们联系，祝您购物愉快！</font> <br />
                        <br />
                        <strong>$SiteName$</strong></td>
                    </tr>
                </tbody>
            </table>
            </td>
        </tr>
        <tr>
            <td bgcolor="#ffffff">
            <table border="0" cellspacing="0" cellpadding="10">
                <tbody>
                    <tr>
                        <td valign="top" width="396"><font size="2" face="Verdana, Arial, Helvetica, sans-serif">■重要信息，请妥善保存！</font></td>
                    </tr>
                </tbody>
            </table>
            </td>
        </tr>
    </tbody>
</table>', N'订单已提交成功', N'感谢您的订购，您的订单已经提交成功，订单号：$OrderId$。', N'感谢您的订购，您的订单已经提交成功，订单号：$OrderId$。')
INSERT [dbo].[Hishop_MessageTemplates] ([MessageType], [Name], [SendEmail], [SendSMS], [SendInnerMessage], [TagDescription], [EmailSubject], [EmailBody], [InnerMessageSubject], [InnerMessageBody], [SMSBody]) VALUES (N'OrderPayment', N'订单支付时', 0, 0, 0, N'<TABLE border=0 cellSpacing=0 cellPadding=2 width="650" align=left>
        <TBODY>
          <TR>
            <TD width="33%">用户名：$Username$</TD>
            <TD width="33%">订单号：$OrderId$</TD>
            <TD>订单金额：$Total$</TD>
          </TR>
          <TR>
            <TD>店铺名称：$SiteName$</TD>
            <TD>&nbsp;</TD>
            <TD>&nbsp;</TD>
          </TR>
        </TBODY>
      </TABLE>', N'订单状态已改变为：买家已付款', N'<table border="0" cellspacing="1" cellpadding="0" style="border-bottom: #336699 1px solid; border-left: #336699 1px solid; border-top: #336699 1px solid; border-right: #336699 1px solid">
    <tbody>
        <tr>
            <td style="border-bottom: #336699 1px solid">
            <table border="0" cellspacing="0" cellpadding="10" width="580">
                <tbody>
                    <tr>
                        <td valign="top" width="261">
                        <p><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2"><strong>Dear $Username$</strong>&nbsp;，<br />
                        <br />
                        您好！感谢您的订购，我们已经收到您的订单款项，并会尽快为您发货。祝您购物愉快！</font></font> <br />
                        <br />
                        <strong>$SiteName$</strong></p>
                        </td>
                        <td valign="top" width="279">
                        <table border="0" cellspacing="0" cellpadding="3" width="100%" bgcolor="#ffffff" align="center">
                            <tbody>
                                <tr>
                                    <td bgcolor="#e1edfe" align="center"><font color="#003366" size="2" face="Verdana, Arial, Helvetica, sans-serif"><b>订单信息</b></font></td>
                                </tr>
                                <tr>
                                    <td bgcolor="#ffffee">
                                    <table border="0" cellspacing="0" cellpadding="2" width="100%" align="left">
                                        <tbody>
                                            <tr>
                                                <td class="small" width="29%">
                                                <div align="right"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2"><strong>订单号：</strong> </font></font></div>
                                                </td>
                                                <td class="small" width="71%"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2">$OrderId$</font></font></td>
                                            </tr>
                                            <tr>
                                                <td class="small">
                                                <div align="right"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2"><strong>付款金额：</strong></font></font></div>
                                                </td>
                                                <td class="small"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2">$Total$</font></font></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td bgcolor="#ffffee">&nbsp;</td>
                                </tr>
                            </tbody>
                        </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            </td>
        </tr>
        <tr>
            <td bgcolor="#ffffff">
            <table border="0" cellspacing="0" cellpadding="10">
                <tbody>
                    <tr>
                        <td valign="top" width="396"><font size="2" face="Verdana, Arial, Helvetica, sans-serif">■重要信息，请妥善保存！</font></td>
                    </tr>
                </tbody>
            </table>
            </td>
        </tr>
    </tbody>
</table>', N'订单状态已改变为：买家已付款', N'您的订单已经成功支付，订单号：$OrderId$，订单金额：$Total$。', N'您的订单已经成功支付，订单号：$OrderId$，订单金额：$Total$。')
INSERT [dbo].[Hishop_MessageTemplates] ([MessageType], [Name], [SendEmail], [SendSMS], [SendInnerMessage], [TagDescription], [EmailSubject], [EmailBody], [InnerMessageSubject], [InnerMessageBody], [SMSBody]) VALUES (N'OrderRefund', N'订单退款以后', 0, 0, 0, N'<TABLE border=0 cellSpacing=0 cellPadding=2 width="650" align=left>
        <TBODY>
          <TR>
            <TD width="33%">用户名：$Username$</TD>
            <TD width="33%">订单号：$OrderId$</TD>
            <TD>退款金额：$RefundMoney$</TD>
          </TR>
          <TR>
            <TD>店铺名称：$SiteName$</TD>
            <TD>&nbsp;</TD>
            <TD>&nbsp;</TD>
          </TR>
        </TBODY>
      </TABLE>', N'订单已退款', N'<table border="0" cellspacing="1" cellpadding="0" style="border-bottom: #336699 1px solid; border-left: #336699 1px solid; border-top: #336699 1px solid; border-right: #336699 1px solid">
    <tbody>
        <tr>
            <td style="border-bottom: #336699 1px solid">
            <table border="0" cellspacing="0" cellpadding="10" width="580">
                <tbody>
                    <tr>
                        <td valign="top" width="261">
                        <p><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2"><strong>Dear $Username$</strong> ，<br />
                        <br />
                        您好！我们已经为您办理了订单退款业务，请查看右侧相关信息，欢迎您继续选购其他商品，祝您购物愉快！</font></font></p>
                        <strong>$SiteName$</strong></td>
                        <td valign="top" width="279">
                        <table border="0" cellspacing="0" cellpadding="3" width="100%" bgcolor="#ffffff" align="center">
                            <tbody>
                                <tr>
                                    <td bgcolor="#e1edfe" align="center"><font color="#003366" size="2" face="Verdana, Arial, Helvetica, sans-serif"><b>订单信息</b></font></td>
                                </tr>
                                <tr>
                                    <td bgcolor="#ffffee">
                                    <table border="0" cellspacing="0" cellpadding="2" width="100%" align="left">
                                        <tbody>
                                            <tr>
                                                <td class="small" width="29%">
                                                <div align="right"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2"><strong>订单号：</strong> </font></font></div>
                                                </td>
                                                <td class="small" width="71%"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2">$OrderId$</font></font></td>
                                            </tr>
                                            <tr>
                                                <td class="small">
                                                <div align="right"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2"><strong>退款金额：</strong></font></font></div>
                                                </td>
                                                <td class="small"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2">$RefundMoney$</font></font></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td bgcolor="#ffffee">&nbsp;</td>
                                </tr>
                            </tbody>
                        </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            </td>
        </tr>
        <tr>
            <td bgcolor="#ffffff">
            <table border="0" cellspacing="0" cellpadding="10">
                <tbody>
                    <tr>
                        <td valign="top" width="396"><font size="2" face="Verdana, Arial, Helvetica, sans-serif">■重要信息，请妥善保存！</font></td>
                    </tr>
                </tbody>
            </table>
            </td>
        </tr>
    </tbody>
</table>', N'订单已退款', N'我们已经为您处理了订单退款相关业务，订单号：$OrderId$，退款金额：$RefundMoney$。', N'我们已经为您处理了订单退款相关业务，订单号：$OrderId$，退款金额：$RefundMoney$。')
INSERT [dbo].[Hishop_MessageTemplates] ([MessageType], [Name], [SendEmail], [SendSMS], [SendInnerMessage], [TagDescription], [EmailSubject], [EmailBody], [InnerMessageSubject], [InnerMessageBody], [SMSBody]) VALUES (N'OrderShipping', N'订单发货时', 0, 0, 0, N'<TABLE border=0 cellSpacing=0 cellPadding=2 width="650" align=left><TBODY><TR><TD width="33%">用户名：$Username$</TD><TD width="33%">订单号：$OrderId$</TD><TD>收货人姓名：$Shipping_Name$</TD></TR><TR><TD>配送方式：$Shipping_Type$</TD><TD>物流单号：$Shipping_Billno$</TD><TD>收货人手机：$Shipping_Cell$</TD></TR><TR><TD>收货人地址：$Shipping_Addr$</TD><TD>收货人邮编：$Shipping_Zip$</TD><TD>收货人电话：$Shipping_Phone$</TD></TR><TR><TD>收货人Email：$Shipping_Email$</TD><TD>店铺名称：$SiteName$</TD><TD>&nbsp;</TD></TR></TBODY></TABLE>', N'订单状态已改变为：卖家已发货', N'<table border="0" cellspacing="1" cellpadding="0" style="border-bottom: #336699 1px solid; border-left: #336699 1px solid; border-top: #336699 1px solid; border-right: #336699 1px solid">
    <tbody>
        <tr>
            <td style="border-bottom: #336699 1px solid">
            <table border="0" cellspacing="0" cellpadding="10" width="580">
                <tbody>
                    <tr>
                        <td valign="top">
                        <p><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2"><strong>Dear $Username$</strong>&nbsp;，<br />
                        <br />
                        您好！感谢您的订购！<br />
                        我们很高兴的通知您，根据您的订购信息，我们已经将您订购的商品寄出，请您注意查收。 <br />
                        </font></font></p>
                        <table border="0" cellspacing="0" cellpadding="3" width="100%" bgcolor="#ffffff" align="center">
                            <tbody>
                                <tr>
                                    <td bgcolor="#e1edfe" align="center"><font color="#003366" size="2" face="Verdana, Arial, Helvetica, sans-serif"><b>订单信息</b></font></td>
                                </tr>
                                <tr>
                                    <td bgcolor="#ffffee">
                                    <table border="0" cellspacing="0" cellpadding="2" width="100%" align="left">
                                        <tbody>
                                            <tr>
                                                <td class="small" width="15%">
                                                <div align="right"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2"><strong>订单号：</strong> </font></font></div>
                                                </td>
                                                <td class="small" width="85%"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2">$OrderId$</font></font></td>
                                            </tr>
                                            <tr>
                                                <td class="small">
                                                <div align="right"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2"><strong>配送方式：</strong></font></font></div>
                                                </td>
                                                <td class="small"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2">$Shipping_Type$</font></font></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                <div align="right"><font size="2"><strong>物流单号：</strong></font></div>
                                                </td>
                                                <td><font size="2" face="Verdana, Arial, Helvetica, sans-serif">$Shipping_Billno$</font></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                <div align="right"><font size="2"><strong>收货人：</strong></font></div>
                                                </td>
                                                <td><font size="2" face="Verdana, Arial, Helvetica, sans-serif">$Shipping_Name$</font></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                <div align="right"><font size="2"><strong>送货地址：</strong></font></div>
                                                </td>
                                                <td><font size="2" face="Verdana, Arial, Helvetica, sans-serif">$Shipping_Addr$</font></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td bgcolor="#ffffee">&nbsp;</td>
                                </tr>
                            </tbody>
                        </table>
                        <font size="2"><br />
                        欢迎您在遇到问题的时候与我们联系，祝您购物愉快！</font> <br />
                        <br />
                        <strong>$SiteName$</strong></td>
                    </tr>
                </tbody>
            </table>
            </td>
        </tr>
        <tr>
            <td bgcolor="#ffffff">
            <table border="0" cellspacing="0" cellpadding="10">
                <tbody>
                    <tr>
                        <td valign="top" width="396"><font size="2" face="Verdana, Arial, Helvetica, sans-serif">■重要信息，请妥善保存！</font></td>
                    </tr>
                </tbody>
            </table>
            </td>
        </tr>
    </tbody>
</table>', N'订单状态已改变为：卖家已发货', N'我们很高兴的通知您，您订购的商品已经寄出，请您注意查收，订单号：$OrderId$。', N'我们很高兴的通知您，您订购的商品已经寄出，请您注意查收，订单号：$OrderId$。')
INSERT [dbo].[Hishop_MessageTemplates] ([MessageType], [Name], [SendEmail], [SendSMS], [SendInnerMessage], [TagDescription], [EmailSubject], [EmailBody], [InnerMessageSubject], [InnerMessageBody], [SMSBody]) VALUES (N'AcceptDistributorRequest', N'分销商审核通过', 1, 0, 0, N'<TABLE border=0 cellSpacing=0 cellPadding=2 width="600" align=left>
        <TBODY>
          <TR>
            <TD width="33%">用户名：$Username$</TD>
            <TD width="33%">邮箱：$Email$</TD>
          </TR>
          <TR>
            <TD>店铺名称：$SiteName$</TD>
            <TD>&nbsp;</TD>
            <TD>&nbsp;</TD>
          </TR>
          <TR>
            <TD colspan="3">&nbsp;</TD>
          </TR>
        </TBODY>
      </TABLE>', N'分销商审核通过', N'<table border="0" cellspacing="1" cellpadding="0" style="border-bottom: #336699 1px solid; border-left: #336699 1px solid; border-top: #336699 1px solid; border-right: #336699 1px solid">
    <tbody>
        <tr>
            <td style="border-bottom: #336699 1px solid">
            <table border="0" cellspacing="0" cellpadding="10" width="580">
                <tbody>
                    <tr>
                        <td valign="top" width="261">
                        <p><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2"><strong>Dear $Username$</strong> ，<br />
                        <br />
                        分销商审核通过！</font><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2">您可以随时进入分销商管理后台查看或修改您的个人资料</font></font><font size="2">。祝您分销顺利！</font></font></p>
                        <strong>$SiteName$</strong></td>
                        <td valign="top" width="279">
                        <table border="0" cellspacing="0" cellpadding="3" width="100%" bgcolor="#ffffff" align="center">
                            <tbody>
                                <tr>
                                    <td bgcolor="#e1edfe" align="center"><font color="#003366" size="2" face="Verdana, Arial, Helvetica, sans-serif"><b>分销商信息</b></font></td>
                                </tr>
                                <tr>
                                    <td bgcolor="#ffffee">
                                    <table border="0" cellspacing="0" cellpadding="2" width="100%" align="left">
                                        <tbody>
                                            <tr>
                                                <td class="small" width="31%">
                                                <div align="right"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2"><strong>用户名：</strong> </font></font></div>
                                                </td>
                                                <td class="small" width="69%"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2">$Username$</font></font></td>
                                            </tr>                                            
                                            <tr>
                                                <td class="small">
                                                <div align="right"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2"><strong>电子邮箱：</strong></font></font></div>
                                                </td>
                                                <td class="small"><font face="Verdana, Arial, Helvetica, sans-serif"><font size="2">$Email$</font></font></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td bgcolor="#ffffee">&nbsp;</td>
                                </tr>
                            </tbody>
                        </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            </td>
        </tr>
        <tr>
            <td bgcolor="#ffffff">
            <table border="0" cellspacing="0" cellpadding="10">
                <tbody>
                    <tr>
                        <td valign="top" width="396"><font size="2" face="Verdana, Arial, Helvetica, sans-serif">■重要信息：此邮件包含个人机密信息，请妥善保存！</font></td>
                    </tr>
                </tbody>
            </table>
            </td>
        </tr>
    </tbody>
</table>', N'分销商审核通过', N'分销商审核通过，您的用户名：$Username$，电子邮件：$Email$。请妥善保管好您的信息。', N'分销商审核通过，您的用户名：$Username$，电子邮件：$Email$。请妥善保管好您的注册信息。')
GO
/****** Object:  Table [dbo].[Hishop_FriendlyLinks]    Script Date: 07/15/2010 10:51:21 ******/
SET IDENTITY_INSERT [dbo].[Hishop_FriendlyLinks] ON
INSERT [dbo].[Hishop_FriendlyLinks] ([LinkId], [ImageUrl], [LinkUrl], [Title], [Visible], [DisplaySequence]) VALUES (2, N'/Storage/master/link/106-402.jpg', N'http://www.itrust.org.cn', N'中国互联网信用评价中心  网络诚信联盟', 1,6)
INSERT [dbo].[Hishop_FriendlyLinks] ([LinkId], [ImageUrl], [LinkUrl], [Title], [Visible], [DisplaySequence]) VALUES (3, N'', N'http://www.17maihuo.com', N'网店货源网', 1, 5)
INSERT [dbo].[Hishop_FriendlyLinks] ([LinkId], [ImageUrl], [LinkUrl], [Title], [Visible], [DisplaySequence]) VALUES (4, N'', N'http://www.hishop.com.cn', N'Hishop商城系统', 1, 4)
INSERT [dbo].[Hishop_FriendlyLinks] ([LinkId], [ImageUrl], [LinkUrl], [Title], [Visible], [DisplaySequence]) VALUES (5, N'', N'http://www.shopefx.com', N'易分销', 1, 3)
INSERT [dbo].[Hishop_FriendlyLinks] ([LinkId], [ImageUrl], [LinkUrl], [Title], [Visible], [DisplaySequence]) VALUES (6, N'', N'http://new.35hiw.com', N'网店主机', 1, 1)
INSERT [dbo].[Hishop_FriendlyLinks] ([LinkId], [ImageUrl], [LinkUrl], [Title], [Visible], [DisplaySequence]) VALUES (8, N'', N'http://www.hishop.com.cn/', N'Hishop网店系统', 1, 2)
INSERT [dbo].[Hishop_FriendlyLinks] ([LinkId], [ImageUrl], [LinkUrl], [Title], [Visible], [DisplaySequence]) VALUES (9, N'', N'http://www.53gou.com/', N'网店货源', 1, 7)
SET IDENTITY_INSERT [dbo].[Hishop_FriendlyLinks] OFF

/****** 对象:  Table [dbo].[Hishop_ExpressTemplates]    脚本日期: 11/17/2010 17:47:26 ******/
SET IDENTITY_INSERT [dbo].[Hishop_ExpressTemplates] ON
INSERT [dbo].[Hishop_ExpressTemplates] ([ExpressId], [ExpressName], [XmlFile], [IsUse]) VALUES (2, N'圆通速递', N'201046133312.xml', 1)
INSERT [dbo].[Hishop_ExpressTemplates] ([ExpressId], [ExpressName], [XmlFile], [IsUse]) VALUES (3, N'申通物流', N'20101117152320.xml', 1)
INSERT [dbo].[Hishop_ExpressTemplates] ([ExpressId], [ExpressName], [XmlFile], [IsUse]) VALUES (4, N'邮政平邮', N'20101117155058.xml', 1)
INSERT [dbo].[Hishop_ExpressTemplates] ([ExpressId], [ExpressName], [XmlFile], [IsUse]) VALUES (5, N'海航天天快递', N'20101117155351.xml', 1)
INSERT [dbo].[Hishop_ExpressTemplates] ([ExpressId], [ExpressName], [XmlFile], [IsUse]) VALUES (6, N'宅急送', N'20101117155840.xml', 1)
INSERT [dbo].[Hishop_ExpressTemplates] ([ExpressId], [ExpressName], [XmlFile], [IsUse]) VALUES (8, N'韵达快运', N'20101117161058.xml', 1)
INSERT [dbo].[Hishop_ExpressTemplates] ([ExpressId], [ExpressName], [XmlFile], [IsUse]) VALUES (9, N'EMS', N'20101117162921.xml', 1)
INSERT [dbo].[Hishop_ExpressTemplates] ([ExpressId], [ExpressName], [XmlFile], [IsUse]) VALUES (10, N'顺丰快递', N'20101117163245.xml', 1)
INSERT [dbo].[Hishop_ExpressTemplates] ([ExpressId], [ExpressName], [XmlFile], [IsUse]) VALUES (11, N'中通速递', N'20101117163638.xml', 1)
SET IDENTITY_INSERT [dbo].[Hishop_ExpressTemplates] OFF