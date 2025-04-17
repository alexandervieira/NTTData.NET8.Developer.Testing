-- Inserção de categorias
INSERT INTO "Categorias" ("Id", "Name", "Code") VALUES 
('b18f1b37-a19e-469a-a5c1-64f42aac4ed2', 'Adesivos', 102),
('3f24e628-4d4d-4fd4-836f-74260db1033b', 'Bones', 103),
('3e0f3705-05a6-452b-9346-781330b9eee0', 'Camisas', 100),
('88fa66ef-4a37-4f65-b54c-7b235945dc2d', 'Canecas', 101),
('d0dff668-4ff0-4fe9-a92d-bacbd393c234', 'Iphone', 105),
('c6a17573-9e7f-43db-8943-fba457fc0a3c', 'Smartphone', 104);

-- Inserção de produtos
INSERT INTO "Produtos" (
    "Id", "CategoryId", "Title", "Description", "Price", "Image", "QuantityStock", "Rate", "Rating_Count", "Active", "Height", "Width", "Depth"
) VALUES 
('f836df52-fbfa-42be-a45f-019320572ba0', 'd0dff668-4ff0-4fe9-a92d-bacbd393c234', 'IPhone*', 'Aliquam erat volutpat *', true, 1998.00, '2024-03-04T23:59:19.3277662', 'iphone.png', 20, 5, 5, 5),
('731380bf-e48d-4601-87e4-031345aa2edc', 'c6a17573-9e7f-43db-8943-fba457fc0a3c', 'Samsung Galaxy S4*', 'Aliquam erat volutpat *', true, 1199.00, '2024-03-04T23:59:19.3487761', 'galaxy-s4.jpg', 20, 5, 5, 5),
('fcd9c1ef-6ac2-46a9-a09f-176adda22fc6', 'c6a17573-9e7f-43db-8943-fba457fc0a3c', 'Z1*', 'Aliquam erat volutpat *', true, 1389.00, '2024-03-04T23:59:19.3879958', 'Z1.png', 0, 5, 5, 5),
('baee72ba-275a-475f-9653-20aaf46ff7d8', 'c6a17573-9e7f-43db-8943-fba457fc0a3c', 'Samsung Galaxy S4', 'Aliquam erat volutpat', true, 989.00, '2024-03-04T23:59:22.2627261', 'galaxy-s4.jpg', 20, 5, 5, 5),
('ccbbb2e8-19d0-4334-bfd9-3452f4b6323d', '3e0f3705-05a6-452b-9346-781330b9eee0', 'Camiseta Code', 'Camiseta 100% algodão', true, 89.00, '2024-03-04T23:59:12.0215555', 'camiseta2.jpg', 0, 5, 5, 5),
('d9a67134-4c38-41e2-b78f-4f462a312db3', '88fa66ef-4a37-4f65-b54c-7b235945dc2d', 'Caneca StarBugs', 'Aliquam erat volutpat', true, 49.00, '2024-03-04T23:59:12.0417492', 'caneca1.jpg', 0, 5, 5, 5),
('0255161b-62b3-4193-9649-59abd5f36b38', 'd0dff668-4ff0-4fe9-a92d-bacbd393c234', 'IPhone', 'Aliquam erat volutpat', true, 2998.00, '2024-03-04T23:59:22.2392842', 'iphone.png', 0, 5, 5, 5),
('763cdb88-38f9-4c39-92ef-65fe78355c79', 'c6a17573-9e7f-43db-8943-fba457fc0a3c', 'Samsung Galaxy Note', 'Aliquam erat volutpat', true, 1179.00, '2024-03-04T23:59:22.2897152', 'galaxy-note.jpg', 0, 5, 5, 5),
('a1a1d175-2b2d-4082-b6b0-8ab4b983de1f', '3e0f3705-05a6-452b-9346-781330b9eee0', 'Camiseta Developer', 'Camiseta 100% algodão', true, 99.00, '2024-03-04T23:59:11.9881543', 'Camiseta1.jpg', 0, 5, 5, 5),
('61b73928-339a-476e-8deb-99497e225143', 'c6a17573-9e7f-43db-8943-fba457fc0a3c', 'Samsung Galaxy Note*', 'Aliquam erat volutpat *', true, 1289.00, '2024-03-04T23:59:19.3684952', 'galaxy-note.jpg', 0, 5, 5, 5),
('b9ebf18e-fdd1-4259-8eea-a32515ba8d1d', '88fa66ef-4a37-4f65-b54c-7b235945dc2d', 'Caneca Code', 'Aliquam erat volutpat', true, 45.00, '2024-03-04T23:59:12.0623081', 'caneca2.jpg', 0, 5, 5, 5),
('c414b743-8e60-475c-b98b-b29616663591', 'c6a17573-9e7f-43db-8943-fba457fc0a3c', 'Z1', 'Aliquam erat volutpat', true, 1089.00, '2024-03-04T23:59:22.3093503', 'Z1.png', 0, 5, 5, 5);
