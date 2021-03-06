USE [Marlin]
GO
DELETE FROM [dbo].[Translation]
GO
DELETE FROM [dbo].[Language]
GO
DELETE FROM [dbo].[ResourceRole]
GO
DELETE FROM [dbo].[Resource]
GO
DELETE FROM [dbo].[Role]
GO
INSERT [dbo].[Role] ([Id], [Name]) VALUES (N'3396ad78-53be-4a29-9638-6f97189be908', N'Administrators')
INSERT [dbo].[Role] ([Id], [Name]) VALUES (N'a8af94a2-44ce-4bee-845d-5083b58f9bcb', N'Users')
GO
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'302e6072-9d32-4244-8c05-00cc70ffd39f', N'system/password.api', N'POST', 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'11c7ce7c-1a51-47de-9822-099d613eed39', N'admin/role.api', N'POST', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'7460e129-58a6-4b15-80b0-2aa4d454d0be', N'admin/language.api', N'PATCH', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'56de8642-a6c3-41bd-9f78-2b16d6027246', N'system/reset.api', N'GET', 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'56195244-b9ae-448d-84f2-42083a372b5c', N'admin/role.resource.api', N'POST', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'2de264a8-894b-4001-8729-49b1ec7ac74c', N'admin/user.unauthorize.api', N'DELETE', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'46d34991-2f15-43b6-8f15-4b73016f1659', N'system/language.api', N'GET', 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'de2f3a52-004f-4876-bd4c-4d55d492b454', N'admin/user.enable.api', N'PATCH', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'4d04d576-afdf-4427-bf9c-4f7e4e77f232', N'admin/user.authorize.api', N'POST', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'45c04395-2d8a-437c-bd26-5f05572fa65e', N'system/reset.api', N'POST', 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'3dba0e37-c201-4d1a-bdd2-60fd64514456', N'admin/language.api', N'DELETE', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'bea8ab23-2114-4f13-ac39-6354ff829706', N'admin/role.resource.api', N'DELETE', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'07d9ebbf-a820-4682-8c78-651cd29e8994', N'admin/translation.api', N'POST', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'8b756f83-1ca6-43c0-8899-6653927e3c66', N'admin/assembly.api', N'DELETE', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'624a6621-cef2-441c-8c89-6a383aeaacc4', N'admin/resource.api', N'PATCH', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'2c46369c-d2a0-4b94-a214-709266990e6e', N'admin/role.assembly.api', N'POST', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'0ede4ff2-b67b-435f-88be-78a130600591', N'admin/assemblies.api', N'GET', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'8b7992ce-2847-4cd1-8880-78d950b6ff73', N'system/info.api', N'GET', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'66faf19f-303b-4a8e-beaa-86e50d80675b', N'admin/language.api', N'POST', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'a0a48a94-42cf-4dcd-b09f-88c3f256ccef', N'admin/resource.api', N'POST', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'eca5adaa-1bbb-43a9-9389-8e3d31c2ae7d', N'admin/role.api', N'DELETE', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'4332acab-a8dd-4819-a144-8ec9cd6c3e76', N'admin/role.assembly.api', N'DELETE', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'7a4e721c-aa44-478a-aad6-9f8d4c042582', N'admin/resources.api', N'GET', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'0047b470-7d1e-4a5b-b79e-a8b04fbe00cb', N'admin/role.api', N'GET', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'ab5276a3-f131-44a0-ac53-add6599dd869', N'admin/resource.api', N'DELETE', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'6c0570d2-0467-4d41-a574-b15b9927e4f0', N'system/signup.api', N'POST', 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'0a19e02f-93ef-4ea8-b22d-b29760f9cda8', N'admin/roles.api', N'GET', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'dd437cd4-9558-4853-a00a-b6cd8e71c094', N'system/property.api', N'POST', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'7373bce4-e7b0-46e7-ac3c-bff8e7fc6084', N'system/refresh.api', N'GET', 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'5abde5c2-a000-426d-9f1d-d107ca7fe79b', N'admin/user.disable.api', N'PATCH', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'c56106e8-15f7-442d-9a67-d2c3cb69328e', N'admin/role.api', N'PATCH', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'090a0dd0-368b-4cca-be1b-d710324e237f', N'admin/assembly.api', N'POST', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'922e4966-aa3c-440b-a5a9-d9213f1bcce0', N'system/login.api', N'POST', 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'ebf69739-42c6-4c3e-ad6d-f4a178927ea4', N'admin/user.api', N'DELETE', 0, NULL, NULL, NULL, NULL)
INSERT [dbo].[Resource] ([Id], [Url], [Method], [IsPublic], [Order], [Title], [Label], [ParentId]) VALUES (N'5d8025fa-6cc0-4277-afff-f779d86f4d63', N'admin/assembly.api', N'PATCH', 0, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[ResourceRole] ([RoleId], [ResourceId]) VALUES (N'a8af94a2-44ce-4bee-845d-5083b58f9bcb', N'8b7992ce-2847-4cd1-8880-78d950b6ff73')
INSERT [dbo].[ResourceRole] ([RoleId], [ResourceId]) VALUES (N'a8af94a2-44ce-4bee-845d-5083b58f9bcb', N'dd437cd4-9558-4853-a00a-b6cd8e71c094')
INSERT [dbo].[ResourceRole] ([RoleId], [ResourceId]) VALUES (N'3396ad78-53be-4a29-9638-6f97189be908', N'11c7ce7c-1a51-47de-9822-099d613eed39')
INSERT [dbo].[ResourceRole] ([RoleId], [ResourceId]) VALUES (N'3396ad78-53be-4a29-9638-6f97189be908', N'7460e129-58a6-4b15-80b0-2aa4d454d0be')
INSERT [dbo].[ResourceRole] ([RoleId], [ResourceId]) VALUES (N'3396ad78-53be-4a29-9638-6f97189be908', N'56195244-b9ae-448d-84f2-42083a372b5c')
INSERT [dbo].[ResourceRole] ([RoleId], [ResourceId]) VALUES (N'3396ad78-53be-4a29-9638-6f97189be908', N'2de264a8-894b-4001-8729-49b1ec7ac74c')
INSERT [dbo].[ResourceRole] ([RoleId], [ResourceId]) VALUES (N'3396ad78-53be-4a29-9638-6f97189be908', N'de2f3a52-004f-4876-bd4c-4d55d492b454')
INSERT [dbo].[ResourceRole] ([RoleId], [ResourceId]) VALUES (N'3396ad78-53be-4a29-9638-6f97189be908', N'4d04d576-afdf-4427-bf9c-4f7e4e77f232')
INSERT [dbo].[ResourceRole] ([RoleId], [ResourceId]) VALUES (N'3396ad78-53be-4a29-9638-6f97189be908', N'3dba0e37-c201-4d1a-bdd2-60fd64514456')
INSERT [dbo].[ResourceRole] ([RoleId], [ResourceId]) VALUES (N'3396ad78-53be-4a29-9638-6f97189be908', N'bea8ab23-2114-4f13-ac39-6354ff829706')
INSERT [dbo].[ResourceRole] ([RoleId], [ResourceId]) VALUES (N'3396ad78-53be-4a29-9638-6f97189be908', N'07d9ebbf-a820-4682-8c78-651cd29e8994')
INSERT [dbo].[ResourceRole] ([RoleId], [ResourceId]) VALUES (N'3396ad78-53be-4a29-9638-6f97189be908', N'8b756f83-1ca6-43c0-8899-6653927e3c66')
INSERT [dbo].[ResourceRole] ([RoleId], [ResourceId]) VALUES (N'3396ad78-53be-4a29-9638-6f97189be908', N'624a6621-cef2-441c-8c89-6a383aeaacc4')
INSERT [dbo].[ResourceRole] ([RoleId], [ResourceId]) VALUES (N'3396ad78-53be-4a29-9638-6f97189be908', N'2c46369c-d2a0-4b94-a214-709266990e6e')
INSERT [dbo].[ResourceRole] ([RoleId], [ResourceId]) VALUES (N'3396ad78-53be-4a29-9638-6f97189be908', N'0ede4ff2-b67b-435f-88be-78a130600591')
INSERT [dbo].[ResourceRole] ([RoleId], [ResourceId]) VALUES (N'3396ad78-53be-4a29-9638-6f97189be908', N'8b7992ce-2847-4cd1-8880-78d950b6ff73')
INSERT [dbo].[ResourceRole] ([RoleId], [ResourceId]) VALUES (N'3396ad78-53be-4a29-9638-6f97189be908', N'66faf19f-303b-4a8e-beaa-86e50d80675b')
INSERT [dbo].[ResourceRole] ([RoleId], [ResourceId]) VALUES (N'3396ad78-53be-4a29-9638-6f97189be908', N'a0a48a94-42cf-4dcd-b09f-88c3f256ccef')
INSERT [dbo].[ResourceRole] ([RoleId], [ResourceId]) VALUES (N'3396ad78-53be-4a29-9638-6f97189be908', N'eca5adaa-1bbb-43a9-9389-8e3d31c2ae7d')
INSERT [dbo].[ResourceRole] ([RoleId], [ResourceId]) VALUES (N'3396ad78-53be-4a29-9638-6f97189be908', N'4332acab-a8dd-4819-a144-8ec9cd6c3e76')
INSERT [dbo].[ResourceRole] ([RoleId], [ResourceId]) VALUES (N'3396ad78-53be-4a29-9638-6f97189be908', N'7a4e721c-aa44-478a-aad6-9f8d4c042582')
INSERT [dbo].[ResourceRole] ([RoleId], [ResourceId]) VALUES (N'3396ad78-53be-4a29-9638-6f97189be908', N'0047b470-7d1e-4a5b-b79e-a8b04fbe00cb')
INSERT [dbo].[ResourceRole] ([RoleId], [ResourceId]) VALUES (N'3396ad78-53be-4a29-9638-6f97189be908', N'ab5276a3-f131-44a0-ac53-add6599dd869')
INSERT [dbo].[ResourceRole] ([RoleId], [ResourceId]) VALUES (N'3396ad78-53be-4a29-9638-6f97189be908', N'0a19e02f-93ef-4ea8-b22d-b29760f9cda8')
INSERT [dbo].[ResourceRole] ([RoleId], [ResourceId]) VALUES (N'3396ad78-53be-4a29-9638-6f97189be908', N'dd437cd4-9558-4853-a00a-b6cd8e71c094')
INSERT [dbo].[ResourceRole] ([RoleId], [ResourceId]) VALUES (N'3396ad78-53be-4a29-9638-6f97189be908', N'5abde5c2-a000-426d-9f1d-d107ca7fe79b')
INSERT [dbo].[ResourceRole] ([RoleId], [ResourceId]) VALUES (N'3396ad78-53be-4a29-9638-6f97189be908', N'c56106e8-15f7-442d-9a67-d2c3cb69328e')
INSERT [dbo].[ResourceRole] ([RoleId], [ResourceId]) VALUES (N'3396ad78-53be-4a29-9638-6f97189be908', N'090a0dd0-368b-4cca-be1b-d710324e237f')
INSERT [dbo].[ResourceRole] ([RoleId], [ResourceId]) VALUES (N'3396ad78-53be-4a29-9638-6f97189be908', N'ebf69739-42c6-4c3e-ad6d-f4a178927ea4')
INSERT [dbo].[ResourceRole] ([RoleId], [ResourceId]) VALUES (N'3396ad78-53be-4a29-9638-6f97189be908', N'5d8025fa-6cc0-4277-afff-f779d86f4d63')
GO
INSERT [dbo].[Language] ([Id], [Name]) VALUES (N'en', N'English')
INSERT [dbo].[Language] ([Id], [Name]) VALUES (N'it', N'Italiano')
GO
INSERT [dbo].[Translation] ([LanguageId], [Original], [Translated]) VALUES (N'en', N'Hello world', N'Hello world')
INSERT [dbo].[Translation] ([LanguageId], [Original], [Translated]) VALUES (N'it', N'Hello world', N'Ciao mondo')
GO
