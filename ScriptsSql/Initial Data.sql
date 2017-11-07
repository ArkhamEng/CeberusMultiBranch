
--==================STATES=================================
SET IDENTITY_INSERT Config.State ON

INSERT INTO Config.State (StateId, Code, Name, ShorName, IsActive) VALUES
(1, '01', 'Aguascalientes', 'Ags.', 1),
(2, '02', 'Baja California', 'BC', 1),
(3, '03', 'Baja California Sur', 'BCS', 1),
(4, '04', 'Campeche', 'Camp.', 1),
(5, '05', 'Coahuila', 'Coah.', 1),
(6, '06', 'Colima', 'Col.', 1),
(7, '07', 'Chiapas', 'Chis.', 1),
(8, '08', 'Chihuahua', 'Chih.', 1),
(9, '09', 'Ciudad de México', 'DF', 1),
(10, '10', 'Durango', 'Dgo.', 1),
(11, '11', 'Guanajuato', 'Gto.', 1),
(12, '12', 'Guerrero', 'Gro.', 1),
(13, '13', 'Hidalgo', 'Hgo.', 1),
(14, '14', 'Jalisco', 'Jal.', 1),
(15, '15', 'México', 'Mex.', 1),
(16, '16', 'Michoacán', 'Mich.', 1),
(17, '17', 'Morelos', 'Mor.', 1),
(18, '18', 'Nayarit', 'Nay.', 1),
(19, '19', 'Nuevo León', 'NL', 1),
(20, '20', 'Oaxaca', 'Oax.', 1),
(21, '21', 'Puebla', 'Pue.', 1),
(22, '22', 'Querétaro', 'Qro.', 1),
(23, '23', 'Quintana Roo', 'Q. Roo', 1),
(24, '24', 'San Luis Potosí', 'SLP', 1),
(25, '25', 'Sinaloa', 'Sin.', 1),
(26, '26', 'Sonora', 'Son.', 1),
(27, '27', 'Tabasco', 'Tab.', 1),
(28, '28', 'Tamaulipas', 'Tamps.', 1),
(29, '29', 'Tlaxcala', 'Tlax.', 1),
(30, '30', 'Veracruz', 'Ver.', 1),
(31, '31', 'Yucatán', 'Yuc.', 1),
(32, '32', 'Zacatecas', 'Zac.', 1);

SET IDENTITY_INSERT Config.State OFF

--====================CITIES===============================
SET IDENTITY_INSERT Config.City ON

INSERT INTO Config.City(CityId,StateId,Code,Name,IsActive) VALUES
(1, 1, '001', 'Aguascalientes', 1),
(2, 1, '002', 'Asientos', 1),
(3, 1, '003', 'Calvillo', 1),
(4, 1, '004', 'Cosío', 1),
(5, 1, '005', 'Jesús María', 1),
(6, 1, '006', 'Pabellón de Arteaga', 1),
(7, 1, '007', 'Rincón de Romos', 1),
(8, 1, '008', 'San José de Gracia', 1),
(9, 1, '009', 'Tepezalá', 1),
(10, 1, '010', 'El Llano', 1),
(11, 1, '011', 'San Francisco de los Romo', 1),
(12, 2, '001', 'Ensenada', 1),
(13, 2, '002', 'Mexicali', 1),
(14, 2, '003', 'Tecate', 1),
(15, 2, '004', 'Tijuana', 1),
(16, 2, '005', 'Playas de Rosarito', 1),
(17, 3, '001', 'Comondú', 1),
(18, 3, '002', 'Mulegé', 1),
(19, 3, '003', 'La Paz', 1),
(20, 3, '008', 'Los Cabos', 1),
(21, 3, '009', 'Loreto', 1),
(22, 4, '001', 'Calkiní', 1),
(23, 4, '002', 'Campeche', 1),
(24, 4, '003', 'Carmen', 1),
(25, 4, '004', 'Champotón', 1),
(26, 4, '005', 'Hecelchakán', 1),
(27, 4, '006', 'Hopelchén', 1),
(28, 4, '007', 'Palizada', 1),
(29, 4, '008', 'Tenabo', 1),
(30, 4, '009', 'Escárcega', 1),
(31, 4, '010', 'Calakmul', 1),
(32, 4, '011', 'Candelaria', 1),
(33, 5, '001', 'Abasolo', 1),
(34, 5, '002', 'Acuña', 1),
(35, 5, '003', 'Allende', 1),
(36, 5, '004', 'Arteaga', 1),
(37, 5, '005', 'Candela', 1),
(38, 5, '006', 'Castaños', 1),
(39, 5, '007', 'Cuatro Ciénegas', 1),
(40, 5, '008', 'Escobedo', 1),
(41, 5, '009', 'Francisco I. Madero', 1),
(42, 5, '010', 'Frontera', 1),
(43, 5, '011', 'General Cepeda', 1),
(44, 5, '012', 'Guerrero', 1),
(45, 5, '013', 'Hidalgo', 1),
(46, 5, '014', 'Jiménez', 1),
(47, 5, '015', 'Juárez', 1),
(48, 5, '016', 'Lamadrid', 1),
(49, 5, '017', 'Matamoros', 1),
(50, 5, '018', 'Monclova', 1),
(51, 5, '019', 'Morelos', 1),
(52, 5, '020', 'Múzquiz', 1),
(53, 5, '021', 'Nadadores', 1),
(54, 5, '022', 'Nava', 1),
(55, 5, '023', 'Ocampo', 1),
(56, 5, '024', 'Parras', 1),
(57, 5, '025', 'Piedras Negras', 1),
(58, 5, '026', 'Progreso', 1),
(59, 5, '027', 'Ramos Arizpe', 1),
(60, 5, '028', 'Sabinas', 1),
(61, 5, '029', 'Sacramento', 1),
(62, 5, '030', 'Saltillo', 1),
(63, 5, '031', 'San Buenaventura', 1),
(64, 5, '032', 'San Juan de Sabinas', 1),
(65, 5, '033', 'San Pedro', 1),
(66, 5, '034', 'Sierra Mojada', 1),
(67, 5, '035', 'Torreón', 1),
(68, 5, '036', 'Viesca', 1),
(69, 5, '037', 'Villa Unión', 1),
(70, 5, '038', 'Zaragoza', 1),
(71, 6, '001', 'Armería', 1),
(72, 6, '002', 'Colima', 1),
(73, 6, '003', 'Comala', 1),
(74, 6, '004', 'Coquimatlán', 1),
(75, 6, '005', 'Cuauhtémoc', 1),
(76, 6, '006', 'Ixtlahuacán', 1),
(77, 6, '007', 'Manzanillo', 1),
(78, 6, '008', 'Minatitlán', 1),
(79, 6, '009', 'Tecomán', 1),
(80, 6, '010', 'Villa de Álvarez', 1),
(81, 7, '001', 'Acacoyagua', 1),
(82, 7, '002', 'Acala', 1),
(83, 7, '003', 'Acapetahua', 1),
(84, 7, '004', 'Altamirano', 1),
(85, 7, '005', 'Amatán', 1),
(86, 7, '006', 'Amatenango de la Frontera', 1),
(87, 7, '007', 'Amatenango del Valle', 1),
(88, 7, '008', 'Angel Albino Corzo', 1),
(89, 7, '009', 'Arriaga', 1),
(90, 7, '010', 'Bejucal de Ocampo', 1),
(91, 7, '011', 'Bella Vista', 1),
(92, 7, '012', 'Berriozábal', 1),
(93, 7, '013', 'Bochil', 1),
(94, 7, '014', 'El Bosque', 1),
(95, 7, '015', 'Cacahoatán', 1),
(96, 7, '016', 'Catazajá', 1),
(97, 7, '017', 'Cintalapa', 1),
(98, 7, '018', 'Coapilla', 1),
(99, 7, '019', 'Comitán de Domínguez', 1),
(100, 7, '020', 'La Concordia', 1),
(101, 7, '021', 'Copainalá', 1),
(102, 7, '022', 'Chalchihuitán', 1),
(103, 7, '023', 'Chamula', 1),
(104, 7, '024', 'Chanal', 1),
(105, 7, '025', 'Chapultenango', 1),
(106, 7, '026', 'Chenalhó', 1),
(107, 7, '027', 'Chiapa de Corzo', 1),
(108, 7, '028', 'Chiapilla', 1),
(109, 7, '029', 'Chicoasén', 1),
(110, 7, '030', 'Chicomuselo', 1),
(111, 7, '031', 'Chilón', 1),
(112, 7, '032', 'Escuintla', 1),
(113, 7, '033', 'Francisco León', 1),
(114, 7, '034', 'Frontera Comalapa', 1),
(115, 7, '035', 'Frontera Hidalgo', 1),
(116, 7, '036', 'La Grandeza', 1),
(117, 7, '037', 'Huehuetán', 1),
(118, 7, '038', 'Huixtán', 1),
(119, 7, '039', 'Huitiupán', 1),
(120, 7, '040', 'Huixtla', 1),
(121, 7, '041', 'La Independencia', 1),
(122, 7, '042', 'Ixhuatán', 1),
(123, 7, '043', 'Ixtacomitán', 1),
(124, 7, '044', 'Ixtapa', 1),
(125, 7, '045', 'Ixtapangajoya', 1),
(126, 7, '046', 'Jiquipilas', 1),
(127, 7, '047', 'Jitotol', 1),
(128, 7, '048', 'Juárez', 1),
(129, 7, '049', 'Larráinzar', 1),
(130, 7, '050', 'La Libertad', 1),
(131, 7, '051', 'Mapastepec', 1),
(132, 7, '052', 'Las Margaritas', 1),
(133, 7, '053', 'Mazapa de Madero', 1),
(134, 7, '054', 'Mazatán', 1),
(135, 7, '055', 'Metapa', 1),
(136, 7, '056', 'Mitontic', 1),
(137, 7, '057', 'Motozintla', 1),
(138, 7, '058', 'Nicolás Ruíz', 1),
(139, 7, '059', 'Ocosingo', 1),
(140, 7, '060', 'Ocotepec', 1),
(141, 7, '061', 'Ocozocoautla de Espinosa', 1),
(142, 7, '062', 'Ostuacán', 1),
(143, 7, '063', 'Osumacinta', 1),
(144, 7, '064', 'Oxchuc', 1),
(145, 7, '065', 'Palenque', 1),
(146, 7, '066', 'Pantelhó', 1),
(147, 7, '067', 'Pantepec', 1),
(148, 7, '068', 'Pichucalco', 1),
(149, 7, '069', 'Pijijiapan', 1),
(150, 7, '070', 'El Porvenir', 1),
(151, 7, '071', 'Villa Comaltitlán', 1),
(152, 7, '072', 'Pueblo Nuevo Solistahuacán', 1),
(153, 7, '073', 'Rayón', 1),
(154, 7, '074', 'Reforma', 1),
(155, 7, '075', 'Las Rosas', 1),
(156, 7, '076', 'Sabanilla', 1),
(157, 7, '077', 'Salto de Agua', 1),
(158, 7, '078', 'San Cristóbal de las Casas', 1),
(159, 7, '079', 'San Fernando', 1),
(160, 7, '080', 'Siltepec', 1),
(161, 7, '081', 'Simojovel', 1),
(162, 7, '082', 'Sitalá', 1),
(163, 7, '083', 'Socoltenango', 1),
(164, 7, '084', 'Solosuchiapa', 1),
(165, 7, '085', 'Soyaló', 1),
(166, 7, '086', 'Suchiapa', 1),
(167, 7, '087', 'Suchiate', 1),
(168, 7, '088', 'Sunuapa', 1),
(169, 7, '089', 'Tapachula', 1),
(170, 7, '090', 'Tapalapa', 1),
(171, 7, '091', 'Tapilula', 1),
(172, 7, '092', 'Tecpatán', 1),
(173, 7, '093', 'Tenejapa', 1),
(174, 7, '094', 'Teopisca', 1),
(175, 7, '096', 'Tila', 1),
(176, 7, '097', 'Tonalá', 1),
(177, 7, '098', 'Totolapa', 1),
(178, 7, '099', 'La Trinitaria', 1),
(179, 7, '100', 'Tumbalá', 1),
(180, 7, '101', 'Tuxtla Gutiérrez', 1),
(181, 7, '102', 'Tuxtla Chico', 1),
(182, 7, '103', 'Tuzantán', 1),
(183, 7, '104', 'Tzimol', 1),
(184, 7, '105', 'Unión Juárez', 1),
(185, 7, '106', 'Venustiano Carranza', 1),
(186, 7, '107', 'Villa Corzo', 1),
(187, 7, '108', 'Villaflores', 1),
(188, 7, '109', 'Yajalón', 1),
(189, 7, '110', 'San Lucas', 1),
(190, 7, '111', 'Zinacantán', 1),
(191, 7, '112', 'San Juan Cancuc', 1),
(192, 7, '113', 'Aldama', 1),
(193, 7, '114', 'Benemérito de las Américas', 1),
(194, 7, '115', 'Maravilla Tenejapa', 1),
(195, 7, '116', 'Marqués de Comillas', 1),
(196, 7, '117', 'Montecristo de Guerrero', 1),
(197, 7, '118', 'San Andrés Duraznal', 1),
(198, 7, '119', 'Santiago el Pinar', 1),
(199, 8, '001', 'Ahumada', 1),
(200, 8, '002', 'Aldama', 1),
(201, 8, '003', 'Allende', 1),
(202, 8, '004', 'Aquiles Serdán', 1),
(203, 8, '005', 'Ascensión', 1),
(204, 8, '006', 'Bachíniva', 1),
(205, 8, '007', 'Balleza', 1),
(206, 8, '008', 'Batopilas', 1),
(207, 8, '009', 'Bocoyna', 1),
(208, 8, '010', 'Buenaventura', 1),
(209, 8, '011', 'Camargo', 1),
(210, 8, '012', 'Carichí', 1),
(211, 8, '013', 'Casas Grandes', 1),
(212, 8, '014', 'Coronado', 1),
(213, 8, '015', 'Coyame del Sotol', 1),
(214, 8, '016', 'La Cruz', 1),
(215, 8, '017', 'Cuauhtémoc', 1),
(216, 8, '018', 'Cusihuiriachi', 1),
(217, 8, '019', 'Chihuahua', 1),
(218, 8, '020', 'Chínipas', 1),
(219, 8, '021', 'Delicias', 1),
(220, 8, '022', 'Dr. Belisario Domínguez', 1),
(221, 8, '023', 'Galeana', 1),
(222, 8, '024', 'Santa Isabel', 1),
(223, 8, '025', 'Gómez Farías', 1),
(224, 8, '026', 'Gran Morelos', 1),
(225, 8, '027', 'Guachochi', 1),
(226, 8, '028', 'Guadalupe', 1),
(227, 8, '029', 'Guadalupe y Calvo', 1),
(228, 8, '030', 'Guazapares', 1),
(229, 8, '031', 'Guerrero', 1),
(230, 8, '032', 'Hidalgo del Parral', 1),
(231, 8, '033', 'Huejotitán', 1),
(232, 8, '034', 'Ignacio Zaragoza', 1),
(233, 8, '035', 'Janos', 1),
(234, 8, '036', 'Jiménez', 1),
(235, 8, '037', 'Juárez', 1),
(236, 8, '038', 'Julimes', 1),
(237, 8, '039', 'López', 1),
(238, 8, '040', 'Madera', 1),
(239, 8, '041', 'Maguarichi', 1),
(240, 8, '042', 'Manuel Benavides', 1),
(241, 8, '043', 'Matachí', 1),
(242, 8, '044', 'Matamoros', 1),
(243, 8, '045', 'Meoqui', 1),
(244, 8, '046', 'Morelos', 1),
(245, 8, '047', 'Moris', 1),
(246, 8, '048', 'Namiquipa', 1),
(247, 8, '049', 'Nonoava', 1),
(248, 8, '050', 'Nuevo Casas Grandes', 1),
(249, 8, '051', 'Ocampo', 1),
(250, 8, '052', 'Ojinaga', 1),
(251, 8, '053', 'Praxedis G. Guerrero', 1),
(252, 8, '054', 'Riva Palacio', 1),
(253, 8, '055', 'Rosales', 1),
(254, 8, '056', 'Rosario', 1),
(255, 8, '057', 'San Francisco de Borja', 1),
(256, 8, '058', 'San Francisco de Conchos', 1),
(257, 8, '059', 'San Francisco del Oro', 1),
(258, 8, '060', 'Santa Bárbara', 1),
(259, 8, '061', 'Satevó', 1),
(260, 8, '062', 'Saucillo', 1),
(261, 8, '063', 'Temósachic', 1),
(262, 8, '064', 'El Tule', 1),
(263, 8, '065', 'Urique', 1),
(264, 8, '066', 'Uruachi', 1),
(265, 8, '067', 'Valle de Zaragoza', 1),
(266, 9, '002', 'Azcapotzalco', 1),
(267, 9, '003', 'Coyoacán', 1),
(268, 9, '004', 'Cuajimalpa de Morelos', 1),
(269, 9, '005', 'Gustavo A. Madero', 1),
(270, 9, '006', 'Iztacalco', 1),
(271, 9, '007', 'Iztapalapa', 1),
(272, 9, '008', 'La Magdalena Contreras', 1),
(273, 9, '009', 'Milpa Alta', 1),
(274, 9, '010', 'Álvaro Obregón', 1),
(275, 9, '011', 'Tláhuac', 1),
(276, 9, '012', 'Tlalpan', 1),
(277, 9, '013', 'Xochimilco', 1),
(278, 9, '014', 'Benito Juárez', 1),
(279, 9, '015', 'Cuauhtémoc', 1),
(280, 9, '016', 'Miguel Hidalgo', 1),
(281, 9, '017', 'Venustiano Carranza', 1),
(282, 10, '001', 'Canatlán', 1),
(283, 10, '002', 'Canelas', 1),
(284, 10, '003', 'Coneto de Comonfort', 1),
(285, 10, '004', 'Cuencamé', 1),
(286, 10, '005', 'Durango', 1),
(287, 10, '006', 'General Simón Bolívar', 1),
(288, 10, '007', 'Gómez Palacio', 1),
(289, 10, '008', 'Guadalupe Victoria', 1),
(290, 10, '009', 'Guanaceví', 1),
(291, 10, '010', 'Hidalgo', 1),
(292, 10, '011', 'Indé', 1),
(293, 10, '012', 'Lerdo', 1),
(294, 10, '013', 'Mapimí', 1),
(295, 10, '014', 'Mezquital', 1),
(296, 10, '015', 'Nazas', 1),
(297, 10, '016', 'Nombre de Dios', 1),
(298, 10, '017', 'Ocampo', 1),
(299, 10, '018', 'El Oro', 1),
(300, 10, '019', 'Otáez', 1),
(301, 10, '020', 'Pánuco de Coronado', 1),
(302, 10, '021', 'Peñón Blanco', 1),
(303, 10, '022', 'Poanas', 1),
(304, 10, '023', 'Pueblo Nuevo', 1),
(305, 10, '024', 'Rodeo', 1),
(306, 10, '025', 'San Bernardo', 1),
(307, 10, '026', 'San Dimas', 1),
(308, 10, '027', 'San Juan de Guadalupe', 1),
(309, 10, '028', 'San Juan del Río', 1),
(310, 10, '029', 'San Luis del Cordero', 1),
(311, 10, '030', 'San Pedro del Gallo', 1),
(312, 10, '031', 'Santa Clara', 1),
(313, 10, '032', 'Santiago Papasquiaro', 1),
(314, 10, '033', 'Súchil', 1),
(315, 10, '034', 'Tamazula', 1),
(316, 10, '035', 'Tepehuanes', 1),
(317, 10, '036', 'Tlahualilo', 1),
(318, 10, '037', 'Topia', 1),
(319, 10, '038', 'Vicente Guerrero', 1),
(320, 10, '039', 'Nuevo Ideal', 1),
(321, 11, '001', 'Abasolo', 1),
(322, 11, '002', 'Acámbaro', 1),
(323, 11, '003', 'San Miguel de Allende', 1),
(324, 11, '004', 'Apaseo el Alto', 1),
(325, 11, '005', 'Apaseo el Grande', 1),
(326, 11, '006', 'Atarjea', 1),
(327, 11, '007', 'Celaya', 1),
(328, 11, '008', 'Manuel Doblado', 1),
(329, 11, '009', 'Comonfort', 1),
(330, 11, '010', 'Coroneo', 1),
(331, 11, '011', 'Cortazar', 1),
(332, 11, '012', 'Cuerámaro', 1),
(333, 11, '013', 'Doctor Mora', 1),
(334, 11, '014', 'Dolores Hidalgo Cuna de la Independencia Nacional', 1),
(335, 11, '015', 'Guanajuato', 1),
(336, 11, '016', 'Huanímaro', 1),
(337, 11, '017', 'Irapuato', 1),
(338, 11, '018', 'Jaral del Progreso', 1),
(339, 11, '019', 'Jerécuaro', 1),
(340, 11, '020', 'León', 1),
(341, 11, '021', 'Moroleón', 1),
(342, 11, '022', 'Ocampo', 1),
(343, 11, '023', 'Pénjamo', 1),
(344, 11, '024', 'Pueblo Nuevo', 1),
(345, 11, '025', 'Purísima del Rincón', 1),
(346, 11, '026', 'Romita', 1),
(347, 11, '027', 'Salamanca', 1),
(348, 11, '028', 'Salvatierra', 1),
(349, 11, '029', 'San Diego de la Unión', 1),
(350, 11, '030', 'San Felipe', 1),
(351, 11, '031', 'San Francisco del Rincón', 1),
(352, 11, '032', 'San José Iturbide', 1),
(353, 11, '033', 'San Luis de la Paz', 1),
(354, 11, '034', 'Santa Catarina', 1),
(355, 11, '035', 'Santa Cruz de Juventino Rosas', 1),
(356, 11, '036', 'Santiago Maravatío', 1),
(357, 11, '037', 'Silao de la Victoria', 1),
(358, 11, '038', 'Tarandacuao', 1),
(359, 11, '039', 'Tarimoro', 1),
(360, 11, '040', 'Tierra Blanca', 1),
(361, 11, '041', 'Uriangato', 1),
(362, 11, '042', 'Valle de Santiago', 1),
(363, 11, '043', 'Victoria', 1),
(364, 11, '044', 'Villagrán', 1),
(365, 11, '045', 'Xichú', 1),
(366, 11, '046', 'Yuriria', 1),
(367, 12, '001', 'Acapulco de Juárez', 1),
(368, 12, '002', 'Ahuacuotzingo', 1),
(369, 12, '003', 'Ajuchitlán del Progreso', 1),
(370, 12, '004', 'Alcozauca de Guerrero', 1),
(371, 12, '005', 'Alpoyeca', 1),
(372, 12, '006', 'Apaxtla', 1),
(373, 12, '007', 'Arcelia', 1),
(374, 12, '008', 'Atenango del Río', 1),
(375, 12, '009', 'Atlamajalcingo del Monte', 1),
(376, 12, '010', 'Atlixtac', 1),
(377, 12, '011', 'Atoyac de Álvarez', 1),
(378, 12, '012', 'Ayutla de los Libres', 1),
(379, 12, '013', 'Azoyú', 1),
(380, 12, '014', 'Benito Juárez', 1),
(381, 12, '015', 'Buenavista de Cuéllar', 1),
(382, 12, '016', 'Coahuayutla de José María Izazaga', 1),
(383, 12, '017', 'Cocula', 1),
(384, 12, '018', 'Copala', 1),
(385, 12, '019', 'Copalillo', 1),
(386, 12, '020', 'Copanatoyac', 1),
(387, 12, '021', 'Coyuca de Benítez', 1),
(388, 12, '022', 'Coyuca de Catalán', 1),
(389, 12, '023', 'Cuajinicuilapa', 1),
(390, 12, '024', 'Cualác', 1),
(391, 12, '025', 'Cuautepec', 1),
(392, 12, '026', 'Cuetzala del Progreso', 1),
(393, 12, '027', 'Cutzamala de Pinzón', 1),
(394, 12, '028', 'Chilapa de Álvarez', 1),
(395, 12, '029', 'Chilpancingo de los Bravo', 1),
(396, 12, '030', 'Florencio Villarreal', 1),
(397, 12, '031', 'General Canuto A. Neri', 1),
(398, 12, '032', 'General Heliodoro Castillo', 1),
(399, 12, '033', 'Huamuxtitlán', 1),
(400, 12, '034', 'Huitzuco de los Figueroa', 1),
(401, 12, '035', 'Iguala de la Independencia', 1),
(402, 12, '036', 'Igualapa', 1),
(403, 12, '037', 'Ixcateopan de Cuauhtémoc', 1),
(404, 12, '038', 'Zihuatanejo de Azueta', 1),
(405, 12, '039', 'Juan R. Escudero', 1),
(406, 12, '040', 'Leonardo Bravo', 1),
(407, 12, '041', 'Malinaltepec', 1),
(408, 12, '042', 'Mártir de Cuilapan', 1),
(409, 12, '043', 'Metlatónoc', 1),
(410, 12, '044', 'Mochitlán', 1),
(411, 12, '045', 'Olinalá', 1),
(412, 12, '046', 'Ometepec', 1),
(413, 12, '047', 'Pedro Ascencio Alquisiras', 1),
(414, 12, '048', 'Petatlán', 1),
(415, 12, '049', 'Pilcaya', 1),
(416, 12, '050', 'Pungarabato', 1),
(417, 12, '051', 'Quechultenango', 1),
(418, 12, '052', 'San Luis Acatlán', 1),
(419, 12, '053', 'San Marcos', 1),
(420, 12, '054', 'San Miguel Totolapan', 1),
(421, 12, '055', 'Taxco de Alarcón', 1),
(422, 12, '056', 'Tecoanapa', 1),
(423, 12, '057', 'Técpan de Galeana', 1),
(424, 12, '058', 'Teloloapan', 1),
(425, 12, '059', 'Tepecoacuilco de Trujano', 1),
(426, 12, '060', 'Tetipac', 1),
(427, 12, '061', 'Tixtla de Guerrero', 1),
(428, 12, '062', 'Tlacoachistlahuaca', 1),
(429, 12, '063', 'Tlacoapa', 1),
(430, 12, '064', 'Tlalchapa', 1),
(431, 12, '065', 'Tlalixtaquilla de Maldonado', 1),
(432, 12, '066', 'Tlapa de Comonfort', 1),
(433, 12, '067', 'Tlapehuala', 1),
(434, 12, '068', 'La Unión de Isidoro Montes de Oca', 1),
(435, 12, '069', 'Xalpatláhuac', 1),
(436, 12, '070', 'Xochihuehuetlán', 1),
(437, 12, '071', 'Xochistlahuaca', 1),
(438, 12, '072', 'Zapotitlán Tablas', 1),
(439, 12, '073', 'Zirándaro', 1),
(440, 12, '074', 'Zitlala', 1),
(441, 12, '075', 'Eduardo Neri', 1),
(442, 12, '076', 'Acatepec', 1),
(443, 12, '077', 'Marquelia', 1),
(444, 12, '078', 'Cochoapa el Grande', 1),
(445, 12, '079', 'José Joaquín de Herrera', 1),
(446, 12, '080', 'Juchitán', 1),
(447, 12, '081', 'Iliatenco', 1),
(448, 13, '001', 'Acatlán', 1),
(449, 13, '002', 'Acaxochitlán', 1),
(450, 13, '003', 'Actopan', 1),
(451, 13, '004', 'Agua Blanca de Iturbide', 1),
(452, 13, '005', 'Ajacuba', 1),
(453, 13, '006', 'Alfajayucan', 1),
(454, 13, '007', 'Almoloya', 1),
(455, 13, '008', 'Apan', 1),
(456, 13, '009', 'El Arenal', 1),
(457, 13, '010', 'Atitalaquia', 1),
(458, 13, '011', 'Atlapexco', 1),
(459, 13, '012', 'Atotonilco el Grande', 1),
(460, 13, '013', 'Atotonilco de Tula', 1),
(461, 13, '014', 'Calnali', 1),
(462, 13, '015', 'Cardonal', 1),
(463, 13, '016', 'Cuautepec de Hinojosa', 1),
(464, 13, '017', 'Chapantongo', 1),
(465, 13, '018', 'Chapulhuacán', 1),
(466, 13, '019', 'Chilcuautla', 1),
(467, 13, '020', 'Eloxochitlán', 1),
(468, 13, '021', 'Emiliano Zapata', 1),
(469, 13, '022', 'Epazoyucan', 1),
(470, 13, '023', 'Francisco I. Madero', 1),
(471, 13, '024', 'Huasca de Ocampo', 1),
(472, 13, '025', 'Huautla', 1),
(473, 13, '026', 'Huazalingo', 1),
(474, 13, '027', 'Huehuetla', 1),
(475, 13, '028', 'Huejutla de Reyes', 1),
(476, 13, '029', 'Huichapan', 1),
(477, 13, '030', 'Ixmiquilpan', 1),
(478, 13, '031', 'Jacala de Ledezma', 1),
(479, 13, '032', 'Jaltocán', 1),
(480, 13, '033', 'Juárez Hidalgo', 1),
(481, 13, '034', 'Lolotla', 1),
(482, 13, '035', 'Metepec', 1),
(483, 13, '036', 'San Agustín Metzquititlán', 1),
(484, 13, '037', 'Metztitlán', 1),
(485, 13, '038', 'Mineral del Chico', 1),
(486, 13, '039', 'Mineral del Monte', 1),
(487, 13, '040', 'La Misión', 1),
(488, 13, '041', 'Mixquiahuala de Juárez', 1),
(489, 13, '042', 'Molango de Escamilla', 1),
(490, 13, '043', 'Nicolás Flores', 1),
(491, 13, '044', 'Nopala de Villagrán', 1),
(492, 13, '045', 'Omitlán de Juárez', 1),
(493, 13, '046', 'San Felipe Orizatlán', 1),
(494, 13, '047', 'Pacula', 1),
(495, 13, '048', 'Pachuca de Soto', 1),
(496, 13, '049', 'Pisaflores', 1),
(497, 13, '050', 'Progreso de Obregón', 1),
(498, 13, '051', 'Mineral de la Reforma', 1),
(499, 13, '052', 'San Agustín Tlaxiaca', 1),
(500, 13, '053', 'San Bartolo Tutotepec', 1),
(501, 13, '054', 'San Salvador', 1),
(502, 13, '055', 'Santiago de Anaya', 1),
(503, 13, '056', 'Santiago Tulantepec de Lugo Guerrero', 1),
(504, 13, '057', 'Singuilucan', 1),
(505, 13, '058', 'Tasquillo', 1),
(506, 13, '059', 'Tecozautla', 1),
(507, 13, '060', 'Tenango de Doria', 1),
(508, 13, '061', 'Tepeapulco', 1),
(509, 13, '062', 'Tepehuacán de Guerrero', 1),
(510, 13, '063', 'Tepeji del Río de Ocampo', 1),
(511, 13, '064', 'Tepetitlán', 1),
(512, 13, '065', 'Tetepango', 1),
(513, 13, '066', 'Villa de Tezontepec', 1),
(514, 13, '067', 'Tezontepec de Aldama', 1),
(515, 13, '068', 'Tianguistengo', 1),
(516, 13, '069', 'Tizayuca', 1),
(517, 13, '070', 'Tlahuelilpan', 1),
(518, 13, '071', 'Tlahuiltepa', 1),
(519, 13, '072', 'Tlanalapa', 1),
(520, 13, '073', 'Tlanchinol', 1),
(521, 13, '074', 'Tlaxcoapan', 1),
(522, 13, '075', 'Tolcayuca', 1),
(523, 13, '076', 'Tula de Allende', 1),
(524, 13, '077', 'Tulancingo de Bravo', 1),
(525, 13, '078', 'Xochiatipan', 1),
(526, 13, '079', 'Xochicoatlán', 1),
(527, 13, '080', 'Yahualica', 1),
(528, 13, '081', 'Zacualtipán de Ángeles', 1),
(529, 13, '082', 'Zapotlán de Juárez', 1),
(530, 13, '083', 'Zempoala', 1),
(531, 13, '084', 'Zimapán', 1),
(532, 14, '001', 'Acatic', 1),
(533, 14, '002', 'Acatlán de Juárez', 1),
(534, 14, '003', 'Ahualulco de Mercado', 1),
(535, 14, '004', 'Amacueca', 1),
(536, 14, '005', 'Amatitán', 1),
(537, 14, '006', 'Ameca', 1),
(538, 14, '007', 'San Juanito de Escobedo', 1),
(539, 14, '008', 'Arandas', 1),
(540, 14, '009', 'El Arenal', 1),
(541, 14, '010', 'Atemajac de Brizuela', 1),
(542, 14, '011', 'Atengo', 1),
(543, 14, '012', 'Atenguillo', 1),
(544, 14, '013', 'Atotonilco el Alto', 1),
(545, 14, '014', 'Atoyac', 1),
(546, 14, '015', 'Autlán de Navarro', 1),
(547, 14, '016', 'Ayotlán', 1),
(548, 14, '017', 'Ayutla', 1),
(549, 14, '018', 'La Barca', 1),
(550, 14, '019', 'Bolaños', 1),
(551, 14, '020', 'Cabo Corrientes', 1),
(552, 14, '021', 'Casimiro Castillo', 1),
(553, 14, '022', 'Cihuatlán', 1),
(554, 14, '023', 'Zapotlán el Grande', 1),
(555, 14, '024', 'Cocula', 1),
(556, 14, '025', 'Colotlán', 1),
(557, 14, '026', 'Concepción de Buenos Aires', 1),
(558, 14, '027', 'Cuautitlán de García Barragán', 1),
(559, 14, '028', 'Cuautla', 1),
(560, 14, '029', 'Cuquío', 1),
(561, 14, '030', 'Chapala', 1),
(562, 14, '031', 'Chimaltitán', 1),
(563, 14, '032', 'Chiquilistlán', 1),
(564, 14, '033', 'Degollado', 1),
(565, 14, '034', 'Ejutla', 1),
(566, 14, '035', 'Encarnación de Díaz', 1),
(567, 14, '036', 'Etzatlán', 1),
(568, 14, '037', 'El Grullo', 1),
(569, 14, '038', 'Guachinango', 1),
(570, 14, '039', 'Guadalajara', 1),
(571, 14, '040', 'Hostotipaquillo', 1),
(572, 14, '041', 'Huejúcar', 1),
(573, 14, '042', 'Huejuquilla el Alto', 1),
(574, 14, '043', 'La Huerta', 1),
(575, 14, '044', 'Ixtlahuacán de los Membrillos', 1),
(576, 14, '045', 'Ixtlahuacán del Río', 1),
(577, 14, '046', 'Jalostotitlán', 1),
(578, 14, '047', 'Jamay', 1),
(579, 14, '048', 'Jesús María', 1),
(580, 14, '049', 'Jilotlán de los Dolores', 1),
(581, 14, '050', 'Jocotepec', 1),
(582, 14, '051', 'Juanacatlán', 1),
(583, 14, '052', 'Juchitlán', 1),
(584, 14, '053', 'Lagos de Moreno', 1),
(585, 14, '054', 'El Limón', 1),
(586, 14, '055', 'Magdalena', 1),
(587, 14, '056', 'Santa María del Oro', 1),
(588, 14, '057', 'La Manzanilla de la Paz', 1),
(589, 14, '058', 'Mascota', 1),
(590, 14, '059', 'Mazamitla', 1),
(591, 14, '060', 'Mexticacán', 1),
(592, 14, '061', 'Mezquitic', 1),
(593, 14, '062', 'Mixtlán', 1),
(594, 14, '063', 'Ocotlán', 1),
(595, 14, '064', 'Ojuelos de Jalisco', 1),
(596, 14, '065', 'Pihuamo', 1),
(597, 14, '066', 'Poncitlán', 1),
(598, 14, '067', 'Puerto Vallarta', 1),
(599, 14, '068', 'Villa Purificación', 1),
(600, 14, '069', 'Quitupan', 1),
(601, 14, '070', 'El Salto', 1),
(602, 14, '071', 'San Cristóbal de la Barranca', 1),
(603, 14, '072', 'San Diego de Alejandría', 1),
(604, 14, '073', 'San Juan de los Lagos', 1),
(605, 14, '074', 'San Julián', 1),
(606, 14, '075', 'San Marcos', 1),
(607, 14, '076', 'San Martín de Bolaños', 1),
(608, 14, '077', 'San Martín Hidalgo', 1),
(609, 14, '078', 'San Miguel el Alto', 1),
(610, 14, '079', 'Gómez Farías', 1),
(611, 14, '080', 'San Sebastián del Oeste', 1),
(612, 14, '081', 'Santa María de los Ángeles', 1),
(613, 14, '082', 'Sayula', 1),
(614, 14, '083', 'Tala', 1),
(615, 14, '084', 'Talpa de Allende', 1),
(616, 14, '085', 'Tamazula de Gordiano', 1),
(617, 14, '086', 'Tapalpa', 1),
(618, 14, '087', 'Tecalitlán', 1),
(619, 14, '088', 'Tecolotlán', 1),
(620, 14, '089', 'Techaluta de Montenegro', 1),
(621, 14, '090', 'Tenamaxtlán', 1),
(622, 14, '091', 'Teocaltiche', 1),
(623, 14, '092', 'Teocuitatlán de Corona', 1),
(624, 14, '093', 'Tepatitlán de Morelos', 1),
(625, 14, '094', 'Tequila', 1),
(626, 14, '095', 'Teuchitlán', 1),
(627, 14, '096', 'Tizapán el Alto', 1),
(628, 14, '097', 'Tlajomulco de Zúñiga', 1),
(629, 14, '098', 'San Pedro Tlaquepaque', 1),
(630, 14, '099', 'Tolimán', 1),
(631, 14, '100', 'Tomatlán', 1),
(632, 14, '101', 'Tonalá', 1),
(633, 14, '102', 'Tonaya', 1),
(634, 14, '103', 'Tonila', 1),
(635, 14, '104', 'Totatiche', 1),
(636, 14, '105', 'Tototlán', 1),
(637, 14, '106', 'Tuxcacuesco', 1),
(638, 14, '107', 'Tuxcueca', 1),
(639, 14, '108', 'Tuxpan', 1),
(640, 14, '109', 'Unión de San Antonio', 1),
(641, 14, '110', 'Unión de Tula', 1),
(642, 14, '111', 'Valle de Guadalupe', 1),
(643, 14, '112', 'Valle de Juárez', 1),
(644, 14, '113', 'San Gabriel', 1),
(645, 14, '114', 'Villa Corona', 1),
(646, 14, '115', 'Villa Guerrero', 1),
(647, 14, '116', 'Villa Hidalgo', 1),
(648, 14, '117', 'Cañadas de Obregón', 1),
(649, 14, '118', 'Yahualica de González Gallo', 1),
(650, 14, '119', 'Zacoalco de Torres', 1),
(651, 14, '120', 'Zapopan', 1),
(652, 14, '121', 'Zapotiltic', 1),
(653, 14, '122', 'Zapotitlán de Vadillo', 1),
(654, 14, '123', 'Zapotlán del Rey', 1),
(655, 14, '124', 'Zapotlanejo', 1),
(656, 14, '125', 'San Ignacio Cerro Gordo', 1),
(657, 15, '001', 'Acambay de Ruíz Castañeda', 1),
(658, 15, '002', 'Acolman', 1),
(659, 15, '003', 'Aculco', 1),
(660, 15, '004', 'Almoloya de Alquisiras', 1),
(661, 15, '005', 'Almoloya de Juárez', 1),
(662, 15, '006', 'Almoloya del Río', 1),
(663, 15, '007', 'Amanalco', 1),
(664, 15, '008', 'Amatepec', 1),
(665, 15, '009', 'Amecameca', 1),
(666, 15, '010', 'Apaxco', 1),
(667, 15, '011', 'Atenco', 1),
(668, 15, '012', 'Atizapán', 1),
(669, 15, '013', 'Atizapán de Zaragoza', 1),
(670, 15, '014', 'Atlacomulco', 1),
(671, 15, '015', 'Atlautla', 1),
(672, 15, '016', 'Axapusco', 1),
(673, 15, '017', 'Ayapango', 1),
(674, 15, '018', 'Calimaya', 1),
(675, 15, '019', 'Capulhuac', 1),
(676, 15, '020', 'Coacalco de Berriozábal', 1),
(677, 15, '021', 'Coatepec Harinas', 1),
(678, 15, '022', 'Cocotitlán', 1),
(679, 15, '023', 'Coyotepec', 1),
(680, 15, '024', 'Cuautitlán', 1),
(681, 15, '025', 'Chalco', 1),
(682, 15, '026', 'Chapa de Mota', 1),
(683, 15, '027', 'Chapultepec', 1),
(684, 15, '028', 'Chiautla', 1),
(685, 15, '029', 'Chicoloapan', 1),
(686, 15, '030', 'Chiconcuac', 1),
(687, 15, '031', 'Chimalhuacán', 1),
(688, 15, '032', 'Donato Guerra', 1),
(689, 15, '033', 'Ecatepec de Morelos', 1),
(690, 15, '034', 'Ecatzingo', 1),
(691, 15, '035', 'Huehuetoca', 1),
(692, 15, '036', 'Hueypoxtla', 1),
(693, 15, '037', 'Huixquilucan', 1),
(694, 15, '038', 'Isidro Fabela', 1),
(695, 15, '039', 'Ixtapaluca', 1),
(696, 15, '040', 'Ixtapan de la Sal', 1),
(697, 15, '041', 'Ixtapan del Oro', 1),
(698, 15, '042', 'Ixtlahuaca', 1),
(699, 15, '043', 'Xalatlaco', 1),
(700, 15, '044', 'Jaltenco', 1),
(701, 15, '045', 'Jilotepec', 1),
(702, 15, '046', 'Jilotzingo', 1),
(703, 15, '047', 'Jiquipilco', 1),
(704, 15, '048', 'Jocotitlán', 1),
(705, 15, '049', 'Joquicingo', 1),
(706, 15, '050', 'Juchitepec', 1),
(707, 15, '051', 'Lerma', 1),
(708, 15, '052', 'Malinalco', 1),
(709, 15, '053', 'Melchor Ocampo', 1),
(710, 15, '054', 'Metepec', 1),
(711, 15, '055', 'Mexicaltzingo', 1),
(712, 15, '056', 'Morelos', 1),
(713, 15, '057', 'Naucalpan de Juárez', 1),
(714, 15, '058', 'Nezahualcóyotl', 1),
(715, 15, '059', 'Nextlalpan', 1),
(716, 15, '060', 'Nicolás Romero', 1),
(717, 15, '061', 'Nopaltepec', 1),
(718, 15, '062', 'Ocoyoacac', 1),
(719, 15, '063', 'Ocuilan', 1),
(720, 15, '064', 'El Oro', 1),
(721, 15, '065', 'Otumba', 1),
(722, 15, '066', 'Otzoloapan', 1),
(723, 15, '067', 'Otzolotepec', 1),
(724, 15, '068', 'Ozumba', 1),
(725, 15, '069', 'Papalotla', 1),
(726, 15, '070', 'La Paz', 1),
(727, 15, '071', 'Polotitlán', 1),
(728, 15, '072', 'Rayón', 1),
(729, 15, '073', 'San Antonio la Isla', 1),
(730, 15, '074', 'San Felipe del Progreso', 1),
(731, 15, '075', 'San Martín de las Pirámides', 1),
(732, 15, '076', 'San Mateo Atenco', 1),
(733, 15, '077', 'San Simón de Guerrero', 1),
(734, 15, '078', 'Santo Tomás', 1),
(735, 15, '079', 'Soyaniquilpan de Juárez', 1),
(736, 15, '080', 'Sultepec', 1),
(737, 15, '081', 'Tecámac', 1),
(738, 15, '082', 'Tejupilco', 1),
(739, 15, '083', 'Temamatla', 1),
(740, 15, '084', 'Temascalapa', 1),
(741, 15, '085', 'Temascalcingo', 1),
(742, 15, '086', 'Temascaltepec', 1),
(743, 15, '087', 'Temoaya', 1),
(744, 15, '088', 'Tenancingo', 1),
(745, 15, '089', 'Tenango del Aire', 1),
(746, 15, '090', 'Tenango del Valle', 1),
(747, 15, '091', 'Teoloyucan', 1),
(748, 15, '092', 'Teotihuacán', 1),
(749, 15, '093', 'Tepetlaoxtoc', 1),
(750, 15, '094', 'Tepetlixpa', 1),
(751, 15, '095', 'Tepotzotlán', 1),
(752, 15, '096', 'Tequixquiac', 1),
(753, 15, '097', 'Texcaltitlán', 1),
(754, 15, '098', 'Texcalyacac', 1),
(755, 15, '099', 'Texcoco', 1),
(756, 15, '100', 'Tezoyuca', 1),
(757, 15, '101', 'Tianguistenco', 1),
(758, 15, '102', 'Timilpan', 1),
(759, 15, '103', 'Tlalmanalco', 1),
(760, 15, '104', 'Tlalnepantla de Baz', 1),
(761, 15, '105', 'Tlatlaya', 1),
(762, 15, '106', 'Toluca', 1),
(763, 15, '107', 'Tonatico', 1),
(764, 15, '108', 'Tultepec', 1),
(765, 15, '109', 'Tultitlán', 1),
(766, 15, '110', 'Valle de Bravo', 1),
(767, 15, '111', 'Villa de Allende', 1),
(768, 15, '112', 'Villa del Carbón', 1),
(769, 15, '113', 'Villa Guerrero', 1),
(770, 15, '114', 'Villa Victoria', 1),
(771, 15, '115', 'Xonacatlán', 1),
(772, 15, '116', 'Zacazonapan', 1),
(773, 15, '117', 'Zacualpan', 1),
(774, 15, '118', 'Zinacantepec', 1),
(775, 15, '119', 'Zumpahuacán', 1),
(776, 15, '120', 'Zumpango', 1),
(777, 15, '121', 'Cuautitlán Izcalli', 1),
(778, 15, '122', 'Valle de Chalco Solidaridad', 1),
(779, 15, '123', 'Luvianos', 1),
(780, 15, '124', 'San José del Rincón', 1),
(781, 15, '125', 'Tonanitla', 1),
(782, 16, '001', 'Acuitzio', 1),
(783, 16, '002', 'Aguililla', 1),
(784, 16, '003', 'Álvaro Obregón', 1),
(785, 16, '004', 'Angamacutiro', 1),
(786, 16, '005', 'Angangueo', 1),
(787, 16, '006', 'Apatzingán', 1),
(788, 16, '007', 'Aporo', 1),
(789, 16, '008', 'Aquila', 1),
(790, 16, '009', 'Ario', 1),
(791, 16, '010', 'Arteaga', 1),
(792, 16, '011', 'Briseñas', 1),
(793, 16, '012', 'Buenavista', 1),
(794, 16, '013', 'Carácuaro', 1),
(795, 16, '014', 'Coahuayana', 1),
(796, 16, '015', 'Coalcomán de Vázquez Pallares', 1),
(797, 16, '016', 'Coeneo', 1),
(798, 16, '017', 'Contepec', 1),
(799, 16, '018', 'Copándaro', 1),
(800, 16, '019', 'Cotija', 1),
(801, 16, '020', 'Cuitzeo', 1),
(802, 16, '021', 'Charapan', 1),
(803, 16, '022', 'Charo', 1),
(804, 16, '023', 'Chavinda', 1),
(805, 16, '024', 'Cherán', 1),
(806, 16, '025', 'Chilchota', 1),
(807, 16, '026', 'Chinicuila', 1),
(808, 16, '027', 'Chucándiro', 1),
(809, 16, '028', 'Churintzio', 1),
(810, 16, '029', 'Churumuco', 1),
(811, 16, '030', 'Ecuandureo', 1),
(812, 16, '031', 'Epitacio Huerta', 1),
(813, 16, '032', 'Erongarícuaro', 1),
(814, 16, '033', 'Gabriel Zamora', 1),
(815, 16, '034', 'Hidalgo', 1),
(816, 16, '035', 'La Huacana', 1),
(817, 16, '036', 'Huandacareo', 1),
(818, 16, '037', 'Huaniqueo', 1),
(819, 16, '038', 'Huetamo', 1),
(820, 16, '039', 'Huiramba', 1),
(821, 16, '040', 'Indaparapeo', 1),
(822, 16, '041', 'Irimbo', 1),
(823, 16, '042', 'Ixtlán', 1),
(824, 16, '043', 'Jacona', 1),
(825, 16, '044', 'Jiménez', 1),
(826, 16, '045', 'Jiquilpan', 1),
(827, 16, '046', 'Juárez', 1),
(828, 16, '047', 'Jungapeo', 1),
(829, 16, '048', 'Lagunillas', 1),
(830, 16, '049', 'Madero', 1),
(831, 16, '050', 'Maravatío', 1),
(832, 16, '051', 'Marcos Castellanos', 1),
(833, 16, '052', 'Lázaro Cárdenas', 1),
(834, 16, '053', 'Morelia', 1),
(835, 16, '054', 'Morelos', 1),
(836, 16, '055', 'Múgica', 1),
(837, 16, '056', 'Nahuatzen', 1),
(838, 16, '057', 'Nocupétaro', 1),
(839, 16, '058', 'Nuevo Parangaricutiro', 1),
(840, 16, '059', 'Nuevo Urecho', 1),
(841, 16, '060', 'Numarán', 1),
(842, 16, '061', 'Ocampo', 1),
(843, 16, '062', 'Pajacuarán', 1),
(844, 16, '063', 'Panindícuaro', 1),
(845, 16, '064', 'Parácuaro', 1),
(846, 16, '065', 'Paracho', 1),
(847, 16, '066', 'Pátzcuaro', 1),
(848, 16, '067', 'Penjamillo', 1),
(849, 16, '068', 'Peribán', 1),
(850, 16, '069', 'La Piedad', 1),
(851, 16, '070', 'Purépero', 1),
(852, 16, '071', 'Puruándiro', 1),
(853, 16, '072', 'Queréndaro', 1),
(854, 16, '073', 'Quiroga', 1),
(855, 16, '074', 'Cojumatlán de Régules', 1),
(856, 16, '075', 'Los Reyes', 1),
(857, 16, '076', 'Sahuayo', 1),
(858, 16, '077', 'San Lucas', 1),
(859, 16, '078', 'Santa Ana Maya', 1),
(860, 16, '079', 'Salvador Escalante', 1),
(861, 16, '080', 'Senguio', 1),
(862, 16, '081', 'Susupuato', 1),
(863, 16, '082', 'Tacámbaro', 1),
(864, 16, '083', 'Tancítaro', 1),
(865, 16, '084', 'Tangamandapio', 1),
(866, 16, '085', 'Tangancícuaro', 1),
(867, 16, '086', 'Tanhuato', 1),
(868, 16, '087', 'Taretan', 1),
(869, 16, '088', 'Tarímbaro', 1),
(870, 16, '089', 'Tepalcatepec', 1),
(871, 16, '090', 'Tingambato', 1),
(872, 16, '091', 'Tingüindín', 1),
(873, 16, '092', 'Tiquicheo de Nicolás Romero', 1),
(874, 16, '093', 'Tlalpujahua', 1),
(875, 16, '094', 'Tlazazalca', 1),
(876, 16, '095', 'Tocumbo', 1),
(877, 16, '096', 'Tumbiscatío', 1),
(878, 16, '097', 'Turicato', 1),
(879, 16, '098', 'Tuxpan', 1),
(880, 16, '099', 'Tuzantla', 1),
(881, 16, '100', 'Tzintzuntzan', 1),
(882, 16, '101', 'Tzitzio', 1),
(883, 16, '102', 'Uruapan', 1),
(884, 16, '103', 'Venustiano Carranza', 1),
(885, 16, '104', 'Villamar', 1),
(886, 16, '105', 'Vista Hermosa', 1),
(887, 16, '106', 'Yurécuaro', 1),
(888, 16, '107', 'Zacapu', 1),
(889, 16, '108', 'Zamora', 1),
(890, 16, '109', 'Zináparo', 1),
(891, 16, '110', 'Zinapécuaro', 1),
(892, 16, '111', 'Ziracuaretiro', 1),
(893, 16, '112', 'Zitácuaro', 1),
(894, 16, '113', 'José Sixto Verduzco', 1),
(895, 17, '001', 'Amacuzac', 1),
(896, 17, '002', 'Atlatlahucan', 1),
(897, 17, '003', 'Axochiapan', 1),
(898, 17, '004', 'Ayala', 1),
(899, 17, '005', 'Coatlán del Río', 1),
(900, 17, '006', 'Cuautla', 1),
(901, 17, '007', 'Cuernavaca', 1),
(902, 17, '008', 'Emiliano Zapata', 1),
(903, 17, '009', 'Huitzilac', 1),
(904, 17, '010', 'Jantetelco', 1),
(905, 17, '011', 'Jiutepec', 1),
(906, 17, '012', 'Jojutla', 1),
(907, 17, '013', 'Jonacatepec', 1),
(908, 17, '014', 'Mazatepec', 1),
(909, 17, '015', 'Miacatlán', 1),
(910, 17, '016', 'Ocuituco', 1),
(911, 17, '017', 'Puente de Ixtla', 1),
(912, 17, '018', 'Temixco', 1),
(913, 17, '019', 'Tepalcingo', 1),
(914, 17, '020', 'Tepoztlán', 1),
(915, 17, '021', 'Tetecala', 1),
(916, 17, '022', 'Tetela del Volcán', 1),
(917, 17, '023', 'Tlalnepantla', 1),
(918, 17, '024', 'Tlaltizapán de Zapata', 1),
(919, 17, '025', 'Tlaquiltenango', 1),
(920, 17, '026', 'Tlayacapan', 1),
(921, 17, '027', 'Totolapan', 1),
(922, 17, '028', 'Xochitepec', 1),
(923, 17, '029', 'Yautepec', 1),
(924, 17, '030', 'Yecapixtla', 1),
(925, 17, '031', 'Zacatepec', 1),
(926, 17, '032', 'Zacualpan de Amilpas', 1),
(927, 17, '033', 'Temoac', 1),
(928, 18, '001', 'Acaponeta', 1),
(929, 18, '002', 'Ahuacatlán', 1),
(930, 18, '003', 'Amatlán de Cañas', 1),
(931, 18, '004', 'Compostela', 1),
(932, 18, '005', 'Huajicori', 1),
(933, 18, '006', 'Ixtlán del Río', 1),
(934, 18, '007', 'Jala', 1),
(935, 18, '008', 'Xalisco', 1),
(936, 18, '009', 'Del Nayar', 1),
(937, 18, '010', 'Rosamorada', 1),
(938, 18, '011', 'Ruíz', 1),
(939, 18, '012', 'San Blas', 1),
(940, 18, '013', 'San Pedro Lagunillas', 1),
(941, 18, '014', 'Santa María del Oro', 1),
(942, 18, '015', 'Santiago Ixcuintla', 1),
(943, 18, '016', 'Tecuala', 1),
(944, 18, '017', 'Tepic', 1),
(945, 18, '018', 'Tuxpan', 1),
(946, 18, '019', 'La Yesca', 1),
(947, 18, '020', 'Bahía de Banderas', 1),
(948, 19, '001', 'Abasolo', 1),
(949, 19, '002', 'Agualeguas', 1),
(950, 19, '003', 'Los Aldamas', 1),
(951, 19, '004', 'Allende', 1),
(952, 19, '005', 'Anáhuac', 1),
(953, 19, '006', 'Apodaca', 1),
(954, 19, '007', 'Aramberri', 1),
(955, 19, '008', 'Bustamante', 1),
(956, 19, '009', 'Cadereyta Jiménez', 1),
(957, 19, '010', 'El Carmen', 1),
(958, 19, '011', 'Cerralvo', 1),
(959, 19, '012', 'Ciénega de Flores', 1),
(960, 19, '013', 'China', 1),
(961, 19, '014', 'Doctor Arroyo', 1),
(962, 19, '015', 'Doctor Coss', 1),
(963, 19, '016', 'Doctor González', 1),
(964, 19, '017', 'Galeana', 1),
(965, 19, '018', 'García', 1),
(966, 19, '019', 'San Pedro Garza García', 1),
(967, 19, '020', 'General Bravo', 1),
(968, 19, '021', 'General Escobedo', 1),
(969, 19, '022', 'General Terán', 1),
(970, 19, '023', 'General Treviño', 1),
(971, 19, '024', 'General Zaragoza', 1),
(972, 19, '025', 'General Zuazua', 1),
(973, 19, '026', 'Guadalupe', 1),
(974, 19, '027', 'Los Herreras', 1),
(975, 19, '028', 'Higueras', 1),
(976, 19, '029', 'Hualahuises', 1),
(977, 19, '030', 'Iturbide', 1),
(978, 19, '031', 'Juárez', 1),
(979, 19, '032', 'Lampazos de Naranjo', 1),
(980, 19, '033', 'Linares', 1),
(981, 19, '034', 'Marín', 1),
(982, 19, '035', 'Melchor Ocampo', 1),
(983, 19, '036', 'Mier y Noriega', 1),
(984, 19, '037', 'Mina', 1),
(985, 19, '038', 'Montemorelos', 1),
(986, 19, '039', 'Monterrey', 1),
(987, 19, '040', 'Parás', 1),
(988, 19, '041', 'Pesquería', 1),
(989, 19, '042', 'Los Ramones', 1),
(990, 19, '043', 'Rayones', 1),
(991, 19, '044', 'Sabinas Hidalgo', 1),
(992, 19, '045', 'Salinas Victoria', 1),
(993, 19, '046', 'San Nicolás de los Garza', 1),
(994, 19, '047', 'Hidalgo', 1),
(995, 19, '048', 'Santa Catarina', 1),
(996, 19, '049', 'Santiago', 1),
(997, 19, '050', 'Vallecillo', 1),
(998, 19, '051', 'Villaldama', 1),
(999, 20, '001', 'Abejones', 1)

INSERT INTO Config.City(CityId,StateId,Code,Name,IsActive) VALUES
(1000, 20, '002', 'Acatlán de Pérez Figueroa', 1),
(1001, 20, '003', 'Asunción Cacalotepec', 1),
(1002, 20, '004', 'Asunción Cuyotepeji', 1),
(1003, 20, '005', 'Asunción Ixtaltepec', 1),
(1004, 20, '006', 'Asunción Nochixtlán', 1),
(1005, 20, '007', 'Asunción Ocotlán', 1),
(1006, 20, '008', 'Asunción Tlacolulita', 1),
(1007, 20, '009', 'Ayotzintepec', 1),
(1008, 20, '010', 'El Barrio de la Soledad', 1),
(1009, 20, '011', 'Calihualá', 1),
(1010, 20, '012', 'Candelaria Loxicha', 1),
(1011, 20, '013', 'Ciénega de Zimatlán', 1),
(1012, 20, '014', 'Ciudad Ixtepec', 1),
(1013, 20, '015', 'Coatecas Altas', 1),
(1014, 20, '016', 'Coicoyán de las Flores', 1),
(1015, 20, '017', 'La Compañía', 1),
(1016, 20, '018', 'Concepción Buenavista', 1),
(1017, 20, '019', 'Concepción Pápalo', 1),
(1018, 20, '020', 'Constancia del Rosario', 1),
(1019, 20, '021', 'Cosolapa', 1),
(1020, 20, '022', 'Cosoltepec', 1),
(1021, 20, '023', 'Cuilápam de Guerrero', 1),
(1022, 20, '024', 'Cuyamecalco Villa de Zaragoza', 1),
(1023, 20, '025', 'Chahuites', 1),
(1024, 20, '026', 'Chalcatongo de Hidalgo', 1),
(1025, 20, '027', 'Chiquihuitlán de Benito Juárez', 1),
(1026, 20, '028', 'Heroica Ciudad de Ejutla de Crespo', 1),
(1027, 20, '029', 'Eloxochitlán de Flores Magón', 1),
(1028, 20, '030', 'El Espinal', 1),
(1029, 20, '031', 'Tamazulápam del Espíritu Santo', 1),
(1030, 20, '032', 'Fresnillo de Trujano', 1),
(1031, 20, '033', 'Guadalupe Etla', 1),
(1032, 20, '034', 'Guadalupe de Ramírez', 1),
(1033, 20, '035', 'Guelatao de Juárez', 1),
(1034, 20, '036', 'Guevea de Humboldt', 1),
(1035, 20, '037', 'Mesones Hidalgo', 1),
(1036, 20, '038', 'Villa Hidalgo', 1),
(1037, 20, '039', 'Heroica Ciudad de Huajuapan de León', 1),
(1038, 20, '040', 'Huautepec', 1),
(1039, 20, '041', 'Huautla de Jiménez', 1),
(1040, 20, '042', 'Ixtlán de Juárez', 1),
(1041, 20, '043', 'Heroica Ciudad de Juchitán de Zaragoza', 1),
(1042, 20, '044', 'Loma Bonita', 1),
(1043, 20, '045', 'Magdalena Apasco', 1),
(1044, 20, '046', 'Magdalena Jaltepec', 1),
(1045, 20, '047', 'Santa Magdalena Jicotlán', 1),
(1046, 20, '048', 'Magdalena Mixtepec', 1),
(1047, 20, '049', 'Magdalena Ocotlán', 1),
(1048, 20, '050', 'Magdalena Peñasco', 1),
(1049, 20, '051', 'Magdalena Teitipac', 1),
(1050, 20, '052', 'Magdalena Tequisistlán', 1),
(1051, 20, '053', 'Magdalena Tlacotepec', 1),
(1052, 20, '054', 'Magdalena Zahuatlán', 1),
(1053, 20, '055', 'Mariscala de Juárez', 1),
(1054, 20, '056', 'Mártires de Tacubaya', 1),
(1055, 20, '057', 'Matías Romero Avendaño', 1),
(1056, 20, '058', 'Mazatlán Villa de Flores', 1),
(1057, 20, '059', 'Miahuatlán de Porfirio Díaz', 1),
(1058, 20, '060', 'Mixistlán de la Reforma', 1),
(1059, 20, '061', 'Monjas', 1),
(1060, 20, '062', 'Natividad', 1),
(1061, 20, '063', 'Nazareno Etla', 1),
(1062, 20, '064', 'Nejapa de Madero', 1),
(1063, 20, '065', 'Ixpantepec Nieves', 1),
(1064, 20, '066', 'Santiago Niltepec', 1),
(1065, 20, '067', 'Oaxaca de Juárez', 1),
(1066, 20, '068', 'Ocotlán de Morelos', 1),
(1067, 20, '069', 'La Pe', 1),
(1068, 20, '070', 'Pinotepa de Don Luis', 1),
(1069, 20, '071', 'Pluma Hidalgo', 1),
(1070, 20, '072', 'San José del Progreso', 1),
(1071, 20, '073', 'Putla Villa de Guerrero', 1),
(1072, 20, '074', 'Santa Catarina Quioquitani', 1),
(1073, 20, '075', 'Reforma de Pineda', 1),
(1074, 20, '076', 'La Reforma', 1),
(1075, 20, '077', 'Reyes Etla', 1),
(1076, 20, '078', 'Rojas de Cuauhtémoc', 1),
(1077, 20, '079', 'Salina Cruz', 1),
(1078, 20, '080', 'San Agustín Amatengo', 1),
(1079, 20, '081', 'San Agustín Atenango', 1),
(1080, 20, '082', 'San Agustín Chayuco', 1),
(1081, 20, '083', 'San Agustín de las Juntas', 1),
(1082, 20, '084', 'San Agustín Etla', 1),
(1083, 20, '085', 'San Agustín Loxicha', 1),
(1084, 20, '086', 'San Agustín Tlacotepec', 1),
(1085, 20, '087', 'San Agustín Yatareni', 1),
(1086, 20, '088', 'San Andrés Cabecera Nueva', 1),
(1087, 20, '089', 'San Andrés Dinicuiti', 1),
(1088, 20, '090', 'San Andrés Huaxpaltepec', 1),
(1089, 20, '091', 'San Andrés Huayápam', 1),
(1090, 20, '092', 'San Andrés Ixtlahuaca', 1),
(1091, 20, '093', 'San Andrés Lagunas', 1),
(1092, 20, '094', 'San Andrés Nuxiño', 1),
(1093, 20, '095', 'San Andrés Paxtlán', 1),
(1094, 20, '096', 'San Andrés Sinaxtla', 1),
(1095, 20, '097', 'San Andrés Solaga', 1),
(1096, 20, '098', 'San Andrés Teotilálpam', 1),
(1097, 20, '099', 'San Andrés Tepetlapa', 1),
(1098, 20, '100', 'San Andrés Yaá', 1),
(1099, 20, '101', 'San Andrés Zabache', 1),
(1100, 20, '102', 'San Andrés Zautla', 1),
(1101, 20, '103', 'San Antonino Castillo Velasco', 1),
(1102, 20, '104', 'San Antonino el Alto', 1),
(1103, 20, '105', 'San Antonino Monte Verde', 1),
(1104, 20, '106', 'San Antonio Acutla', 1),
(1105, 20, '107', 'San Antonio de la Cal', 1),
(1106, 20, '108', 'San Antonio Huitepec', 1),
(1107, 20, '109', 'San Antonio Nanahuatípam', 1),
(1108, 20, '110', 'San Antonio Sinicahua', 1),
(1109, 20, '111', 'San Antonio Tepetlapa', 1),
(1110, 20, '112', 'San Baltazar Chichicápam', 1),
(1111, 20, '113', 'San Baltazar Loxicha', 1),
(1112, 20, '114', 'San Baltazar Yatzachi el Bajo', 1),
(1113, 20, '115', 'San Bartolo Coyotepec', 1),
(1114, 20, '116', 'San Bartolomé Ayautla', 1),
(1115, 20, '117', 'San Bartolomé Loxicha', 1),
(1116, 20, '118', 'San Bartolomé Quialana', 1),
(1117, 20, '119', 'San Bartolomé Yucuañe', 1),
(1118, 20, '120', 'San Bartolomé Zoogocho', 1),
(1119, 20, '121', 'San Bartolo Soyaltepec', 1),
(1120, 20, '122', 'San Bartolo Yautepec', 1),
(1121, 20, '123', 'San Bernardo Mixtepec', 1),
(1122, 20, '124', 'San Blas Atempa', 1),
(1123, 20, '125', 'San Carlos Yautepec', 1),
(1124, 20, '126', 'San Cristóbal Amatlán', 1),
(1125, 20, '127', 'San Cristóbal Amoltepec', 1),
(1126, 20, '128', 'San Cristóbal Lachirioag', 1),
(1127, 20, '129', 'San Cristóbal Suchixtlahuaca', 1),
(1128, 20, '130', 'San Dionisio del Mar', 1),
(1129, 20, '131', 'San Dionisio Ocotepec', 1),
(1130, 20, '132', 'San Dionisio Ocotlán', 1),
(1131, 20, '133', 'San Esteban Atatlahuca', 1),
(1132, 20, '134', 'San Felipe Jalapa de Díaz', 1),
(1133, 20, '135', 'San Felipe Tejalápam', 1),
(1134, 20, '136', 'San Felipe Usila', 1),
(1135, 20, '137', 'San Francisco Cahuacuá', 1),
(1136, 20, '138', 'San Francisco Cajonos', 1),
(1137, 20, '139', 'San Francisco Chapulapa', 1),
(1138, 20, '140', 'San Francisco Chindúa', 1),
(1139, 20, '141', 'San Francisco del Mar', 1),
(1140, 20, '142', 'San Francisco Huehuetlán', 1),
(1141, 20, '143', 'San Francisco Ixhuatán', 1),
(1142, 20, '144', 'San Francisco Jaltepetongo', 1),
(1143, 20, '145', 'San Francisco Lachigoló', 1),
(1144, 20, '146', 'San Francisco Logueche', 1),
(1145, 20, '147', 'San Francisco Nuxaño', 1),
(1146, 20, '148', 'San Francisco Ozolotepec', 1),
(1147, 20, '149', 'San Francisco Sola', 1),
(1148, 20, '150', 'San Francisco Telixtlahuaca', 1),
(1149, 20, '151', 'San Francisco Teopan', 1),
(1150, 20, '152', 'San Francisco Tlapancingo', 1),
(1151, 20, '153', 'San Gabriel Mixtepec', 1),
(1152, 20, '154', 'San Ildefonso Amatlán', 1),
(1153, 20, '155', 'San Ildefonso Sola', 1),
(1154, 20, '156', 'San Ildefonso Villa Alta', 1),
(1155, 20, '157', 'San Jacinto Amilpas', 1),
(1156, 20, '158', 'San Jacinto Tlacotepec', 1),
(1157, 20, '159', 'San Jerónimo Coatlán', 1),
(1158, 20, '160', 'San Jerónimo Silacayoapilla', 1),
(1159, 20, '161', 'San Jerónimo Sosola', 1),
(1160, 20, '162', 'San Jerónimo Taviche', 1),
(1161, 20, '163', 'San Jerónimo Tecóatl', 1),
(1162, 20, '164', 'San Jorge Nuchita', 1),
(1163, 20, '165', 'San José Ayuquila', 1),
(1164, 20, '166', 'San José Chiltepec', 1),
(1165, 20, '167', 'San José del Peñasco', 1),
(1166, 20, '168', 'San José Estancia Grande', 1),
(1167, 20, '169', 'San José Independencia', 1),
(1168, 20, '170', 'San José Lachiguiri', 1),
(1169, 20, '171', 'San José Tenango', 1),
(1170, 20, '172', 'San Juan Achiutla', 1),
(1171, 20, '173', 'San Juan Atepec', 1),
(1172, 20, '174', 'Ánimas Trujano', 1),
(1173, 20, '175', 'San Juan Bautista Atatlahuca', 1),
(1174, 20, '176', 'San Juan Bautista Coixtlahuaca', 1),
(1175, 20, '177', 'San Juan Bautista Cuicatlán', 1),
(1176, 20, '178', 'San Juan Bautista Guelache', 1),
(1177, 20, '179', 'San Juan Bautista Jayacatlán', 1),
(1178, 20, '180', 'San Juan Bautista Lo de Soto', 1),
(1179, 20, '181', 'San Juan Bautista Suchitepec', 1),
(1180, 20, '182', 'San Juan Bautista Tlacoatzintepec', 1),
(1181, 20, '183', 'San Juan Bautista Tlachichilco', 1),
(1182, 20, '184', 'San Juan Bautista Tuxtepec', 1),
(1183, 20, '185', 'San Juan Cacahuatepec', 1),
(1184, 20, '186', 'San Juan Cieneguilla', 1),
(1185, 20, '187', 'San Juan Coatzóspam', 1),
(1186, 20, '188', 'San Juan Colorado', 1),
(1187, 20, '189', 'San Juan Comaltepec', 1),
(1188, 20, '190', 'San Juan Cotzocón', 1),
(1189, 20, '191', 'San Juan Chicomezúchil', 1),
(1190, 20, '192', 'San Juan Chilateca', 1),
(1191, 20, '193', 'San Juan del Estado', 1),
(1192, 20, '194', 'San Juan del Río', 1),
(1193, 20, '195', 'San Juan Diuxi', 1),
(1194, 20, '196', 'San Juan Evangelista Analco', 1),
(1195, 20, '197', 'San Juan Guelavía', 1),
(1196, 20, '198', 'San Juan Guichicovi', 1),
(1197, 20, '199', 'San Juan Ihualtepec', 1),
(1198, 20, '200', 'San Juan Juquila Mixes', 1),
(1199, 20, '201', 'San Juan Juquila Vijanos', 1),
(1200, 20, '202', 'San Juan Lachao', 1),
(1201, 20, '203', 'San Juan Lachigalla', 1),
(1202, 20, '204', 'San Juan Lajarcia', 1),
(1203, 20, '205', 'San Juan Lalana', 1),
(1204, 20, '206', 'San Juan de los Cués', 1),
(1205, 20, '207', 'San Juan Mazatlán', 1),
(1206, 20, '208', 'San Juan Mixtepec', 1),
(1207, 20, '209', 'San Juan Mixtepec', 1),
(1208, 20, '210', 'San Juan Ñumí', 1),
(1209, 20, '211', 'San Juan Ozolotepec', 1),
(1210, 20, '212', 'San Juan Petlapa', 1),
(1211, 20, '213', 'San Juan Quiahije', 1),
(1212, 20, '214', 'San Juan Quiotepec', 1),
(1213, 20, '215', 'San Juan Sayultepec', 1),
(1214, 20, '216', 'San Juan Tabaá', 1),
(1215, 20, '217', 'San Juan Tamazola', 1),
(1216, 20, '218', 'San Juan Teita', 1),
(1217, 20, '219', 'San Juan Teitipac', 1),
(1218, 20, '220', 'San Juan Tepeuxila', 1),
(1219, 20, '221', 'San Juan Teposcolula', 1),
(1220, 20, '222', 'San Juan Yaeé', 1),
(1221, 20, '223', 'San Juan Yatzona', 1),
(1222, 20, '224', 'San Juan Yucuita', 1),
(1223, 20, '225', 'San Lorenzo', 1),
(1224, 20, '226', 'San Lorenzo Albarradas', 1),
(1225, 20, '227', 'San Lorenzo Cacaotepec', 1),
(1226, 20, '228', 'San Lorenzo Cuaunecuiltitla', 1),
(1227, 20, '229', 'San Lorenzo Texmelúcan', 1),
(1228, 20, '230', 'San Lorenzo Victoria', 1),
(1229, 20, '231', 'San Lucas Camotlán', 1),
(1230, 20, '232', 'San Lucas Ojitlán', 1),
(1231, 20, '233', 'San Lucas Quiaviní', 1),
(1232, 20, '234', 'San Lucas Zoquiápam', 1),
(1233, 20, '235', 'San Luis Amatlán', 1),
(1234, 20, '236', 'San Marcial Ozolotepec', 1),
(1235, 20, '237', 'San Marcos Arteaga', 1),
(1236, 20, '238', 'San Martín de los Cansecos', 1),
(1237, 20, '239', 'San Martín Huamelúlpam', 1),
(1238, 20, '240', 'San Martín Itunyoso', 1),
(1239, 20, '241', 'San Martín Lachilá', 1),
(1240, 20, '242', 'San Martín Peras', 1),
(1241, 20, '243', 'San Martín Tilcajete', 1),
(1242, 20, '244', 'San Martín Toxpalan', 1),
(1243, 20, '245', 'San Martín Zacatepec', 1),
(1244, 20, '246', 'San Mateo Cajonos', 1),
(1245, 20, '247', 'Capulálpam de Méndez', 1),
(1246, 20, '248', 'San Mateo del Mar', 1),
(1247, 20, '249', 'San Mateo Yoloxochitlán', 1),
(1248, 20, '250', 'San Mateo Etlatongo', 1),
(1249, 20, '251', 'San Mateo Nejápam', 1),
(1250, 20, '252', 'San Mateo Peñasco', 1),
(1251, 20, '253', 'San Mateo Piñas', 1),
(1252, 20, '254', 'San Mateo Río Hondo', 1),
(1253, 20, '255', 'San Mateo Sindihui', 1),
(1254, 20, '256', 'San Mateo Tlapiltepec', 1),
(1255, 20, '257', 'San Melchor Betaza', 1),
(1256, 20, '258', 'San Miguel Achiutla', 1),
(1257, 20, '259', 'San Miguel Ahuehuetitlán', 1),
(1258, 20, '260', 'San Miguel Aloápam', 1),
(1259, 20, '261', 'San Miguel Amatitlán', 1),
(1260, 20, '262', 'San Miguel Amatlán', 1),
(1261, 20, '263', 'San Miguel Coatlán', 1),
(1262, 20, '264', 'San Miguel Chicahua', 1),
(1263, 20, '265', 'San Miguel Chimalapa', 1),
(1264, 20, '266', 'San Miguel del Puerto', 1),
(1265, 20, '267', 'San Miguel del Río', 1),
(1266, 20, '268', 'San Miguel Ejutla', 1),
(1267, 20, '269', 'San Miguel el Grande', 1),
(1268, 20, '270', 'San Miguel Huautla', 1),
(1269, 20, '271', 'San Miguel Mixtepec', 1),
(1270, 20, '272', 'San Miguel Panixtlahuaca', 1),
(1271, 20, '273', 'San Miguel Peras', 1),
(1272, 20, '274', 'San Miguel Piedras', 1),
(1273, 20, '275', 'San Miguel Quetzaltepec', 1),
(1274, 20, '276', 'San Miguel Santa Flor', 1),
(1275, 20, '277', 'Villa Sola de Vega', 1),
(1276, 20, '278', 'San Miguel Soyaltepec', 1),
(1277, 20, '279', 'San Miguel Suchixtepec', 1),
(1278, 20, '280', 'Villa Talea de Castro', 1),
(1279, 20, '281', 'San Miguel Tecomatlán', 1),
(1280, 20, '282', 'San Miguel Tenango', 1),
(1281, 20, '283', 'San Miguel Tequixtepec', 1),
(1282, 20, '284', 'San Miguel Tilquiápam', 1),
(1283, 20, '285', 'San Miguel Tlacamama', 1),
(1284, 20, '286', 'San Miguel Tlacotepec', 1),
(1285, 20, '287', 'San Miguel Tulancingo', 1),
(1286, 20, '288', 'San Miguel Yotao', 1),
(1287, 20, '289', 'San Nicolás', 1),
(1288, 20, '290', 'San Nicolás Hidalgo', 1),
(1289, 20, '291', 'San Pablo Coatlán', 1),
(1290, 20, '292', 'San Pablo Cuatro Venados', 1),
(1291, 20, '293', 'San Pablo Etla', 1),
(1292, 20, '294', 'San Pablo Huitzo', 1),
(1293, 20, '295', 'San Pablo Huixtepec', 1),
(1294, 20, '296', 'San Pablo Macuiltianguis', 1),
(1295, 20, '297', 'San Pablo Tijaltepec', 1),
(1296, 20, '298', 'San Pablo Villa de Mitla', 1),
(1297, 20, '299', 'San Pablo Yaganiza', 1),
(1298, 20, '300', 'San Pedro Amuzgos', 1),
(1299, 20, '301', 'San Pedro Apóstol', 1),
(1300, 20, '302', 'San Pedro Atoyac', 1),
(1301, 20, '303', 'San Pedro Cajonos', 1),
(1302, 20, '304', 'San Pedro Coxcaltepec Cántaros', 1),
(1303, 20, '305', 'San Pedro Comitancillo', 1),
(1304, 20, '306', 'San Pedro el Alto', 1),
(1305, 20, '307', 'San Pedro Huamelula', 1),
(1306, 20, '308', 'San Pedro Huilotepec', 1),
(1307, 20, '309', 'San Pedro Ixcatlán', 1),
(1308, 20, '310', 'San Pedro Ixtlahuaca', 1),
(1309, 20, '311', 'San Pedro Jaltepetongo', 1),
(1310, 20, '312', 'San Pedro Jicayán', 1),
(1311, 20, '313', 'San Pedro Jocotipac', 1),
(1312, 20, '314', 'San Pedro Juchatengo', 1),
(1313, 20, '315', 'San Pedro Mártir', 1),
(1314, 20, '316', 'San Pedro Mártir Quiechapa', 1),
(1315, 20, '317', 'San Pedro Mártir Yucuxaco', 1),
(1316, 20, '318', 'San Pedro Mixtepec', 1),
(1317, 20, '319', 'San Pedro Mixtepec', 1),
(1318, 20, '320', 'San Pedro Molinos', 1),
(1319, 20, '321', 'San Pedro Nopala', 1),
(1320, 20, '322', 'San Pedro Ocopetatillo', 1),
(1321, 20, '323', 'San Pedro Ocotepec', 1),
(1322, 20, '324', 'San Pedro Pochutla', 1),
(1323, 20, '325', 'San Pedro Quiatoni', 1),
(1324, 20, '326', 'San Pedro Sochiápam', 1),
(1325, 20, '327', 'San Pedro Tapanatepec', 1),
(1326, 20, '328', 'San Pedro Taviche', 1),
(1327, 20, '329', 'San Pedro Teozacoalco', 1),
(1328, 20, '330', 'San Pedro Teutila', 1),
(1329, 20, '331', 'San Pedro Tidaá', 1),
(1330, 20, '332', 'San Pedro Topiltepec', 1),
(1331, 20, '333', 'San Pedro Totolápam', 1),
(1332, 20, '334', 'Villa de Tututepec de Melchor Ocampo', 1),
(1333, 20, '335', 'San Pedro Yaneri', 1),
(1334, 20, '336', 'San Pedro Yólox', 1),
(1335, 20, '337', 'San Pedro y San Pablo Ayutla', 1),
(1336, 20, '338', 'Villa de Etla', 1),
(1337, 20, '339', 'San Pedro y San Pablo Teposcolula', 1),
(1338, 20, '340', 'San Pedro y San Pablo Tequixtepec', 1),
(1339, 20, '341', 'San Pedro Yucunama', 1),
(1340, 20, '342', 'San Raymundo Jalpan', 1),
(1341, 20, '343', 'San Sebastián Abasolo', 1),
(1342, 20, '344', 'San Sebastián Coatlán', 1),
(1343, 20, '345', 'San Sebastián Ixcapa', 1),
(1344, 20, '346', 'San Sebastián Nicananduta', 1),
(1345, 20, '347', 'San Sebastián Río Hondo', 1),
(1346, 20, '348', 'San Sebastián Tecomaxtlahuaca', 1),
(1347, 20, '349', 'San Sebastián Teitipac', 1),
(1348, 20, '350', 'San Sebastián Tutla', 1),
(1349, 20, '351', 'San Simón Almolongas', 1),
(1350, 20, '352', 'San Simón Zahuatlán', 1),
(1351, 20, '353', 'Santa Ana', 1),
(1352, 20, '354', 'Santa Ana Ateixtlahuaca', 1),
(1353, 20, '355', 'Santa Ana Cuauhtémoc', 1),
(1354, 20, '356', 'Santa Ana del Valle', 1),
(1355, 20, '357', 'Santa Ana Tavela', 1),
(1356, 20, '358', 'Santa Ana Tlapacoyan', 1),
(1357, 20, '359', 'Santa Ana Yareni', 1),
(1358, 20, '360', 'Santa Ana Zegache', 1),
(1359, 20, '361', 'Santa Catalina Quierí', 1),
(1360, 20, '362', 'Santa Catarina Cuixtla', 1),
(1361, 20, '363', 'Santa Catarina Ixtepeji', 1),
(1362, 20, '364', 'Santa Catarina Juquila', 1),
(1363, 20, '365', 'Santa Catarina Lachatao', 1),
(1364, 20, '366', 'Santa Catarina Loxicha', 1),
(1365, 20, '367', 'Santa Catarina Mechoacán', 1),
(1366, 20, '368', 'Santa Catarina Minas', 1),
(1367, 20, '369', 'Santa Catarina Quiané', 1),
(1368, 20, '370', 'Santa Catarina Tayata', 1),
(1369, 20, '371', 'Santa Catarina Ticuá', 1),
(1370, 20, '372', 'Santa Catarina Yosonotú', 1),
(1371, 20, '373', 'Santa Catarina Zapoquila', 1),
(1372, 20, '374', 'Santa Cruz Acatepec', 1),
(1373, 20, '375', 'Santa Cruz Amilpas', 1),
(1374, 20, '376', 'Santa Cruz de Bravo', 1),
(1375, 20, '377', 'Santa Cruz Itundujia', 1),
(1376, 20, '378', 'Santa Cruz Mixtepec', 1),
(1377, 20, '379', 'Santa Cruz Nundaco', 1),
(1378, 20, '380', 'Santa Cruz Papalutla', 1),
(1379, 20, '381', 'Santa Cruz Tacache de Mina', 1),
(1380, 20, '382', 'Santa Cruz Tacahua', 1),
(1381, 20, '383', 'Santa Cruz Tayata', 1),
(1382, 20, '384', 'Santa Cruz Xitla', 1),
(1383, 20, '385', 'Santa Cruz Xoxocotlán', 1),
(1384, 20, '386', 'Santa Cruz Zenzontepec', 1),
(1385, 20, '387', 'Santa Gertrudis', 1),
(1386, 20, '388', 'Santa Inés del Monte', 1),
(1387, 20, '389', 'Santa Inés Yatzeche', 1),
(1388, 20, '390', 'Santa Lucía del Camino', 1),
(1389, 20, '391', 'Santa Lucía Miahuatlán', 1),
(1390, 20, '392', 'Santa Lucía Monteverde', 1),
(1391, 20, '393', 'Santa Lucía Ocotlán', 1),
(1392, 20, '394', 'Santa María Alotepec', 1),
(1393, 20, '395', 'Santa María Apazco', 1),
(1394, 20, '396', 'Santa María la Asunción', 1),
(1395, 20, '397', 'Heroica Ciudad de Tlaxiaco', 1),
(1396, 20, '398', 'Ayoquezco de Aldama', 1),
(1397, 20, '399', 'Santa María Atzompa', 1),
(1398, 20, '400', 'Santa María Camotlán', 1),
(1399, 20, '401', 'Santa María Colotepec', 1),
(1400, 20, '402', 'Santa María Cortijo', 1),
(1401, 20, '403', 'Santa María Coyotepec', 1),
(1402, 20, '404', 'Santa María Chachoápam', 1),
(1403, 20, '405', 'Villa de Chilapa de Díaz', 1),
(1404, 20, '406', 'Santa María Chilchotla', 1),
(1405, 20, '407', 'Santa María Chimalapa', 1),
(1406, 20, '408', 'Santa María del Rosario', 1),
(1407, 20, '409', 'Santa María del Tule', 1),
(1408, 20, '410', 'Santa María Ecatepec', 1),
(1409, 20, '411', 'Santa María Guelacé', 1),
(1410, 20, '412', 'Santa María Guienagati', 1),
(1411, 20, '413', 'Santa María Huatulco', 1),
(1412, 20, '414', 'Santa María Huazolotitlán', 1),
(1413, 20, '415', 'Santa María Ipalapa', 1),
(1414, 20, '416', 'Santa María Ixcatlán', 1),
(1415, 20, '417', 'Santa María Jacatepec', 1),
(1416, 20, '418', 'Santa María Jalapa del Marqués', 1),
(1417, 20, '419', 'Santa María Jaltianguis', 1),
(1418, 20, '420', 'Santa María Lachixío', 1),
(1419, 20, '421', 'Santa María Mixtequilla', 1),
(1420, 20, '422', 'Santa María Nativitas', 1),
(1421, 20, '423', 'Santa María Nduayaco', 1),
(1422, 20, '424', 'Santa María Ozolotepec', 1),
(1423, 20, '425', 'Santa María Pápalo', 1),
(1424, 20, '426', 'Santa María Peñoles', 1),
(1425, 20, '427', 'Santa María Petapa', 1),
(1426, 20, '428', 'Santa María Quiegolani', 1),
(1427, 20, '429', 'Santa María Sola', 1),
(1428, 20, '430', 'Santa María Tataltepec', 1),
(1429, 20, '431', 'Santa María Tecomavaca', 1),
(1430, 20, '432', 'Santa María Temaxcalapa', 1),
(1431, 20, '433', 'Santa María Temaxcaltepec', 1),
(1432, 20, '434', 'Santa María Teopoxco', 1),
(1433, 20, '435', 'Santa María Tepantlali', 1),
(1434, 20, '436', 'Santa María Texcatitlán', 1),
(1435, 20, '437', 'Santa María Tlahuitoltepec', 1),
(1436, 20, '438', 'Santa María Tlalixtac', 1),
(1437, 20, '439', 'Santa María Tonameca', 1),
(1438, 20, '440', 'Santa María Totolapilla', 1),
(1439, 20, '441', 'Santa María Xadani', 1),
(1440, 20, '442', 'Santa María Yalina', 1),
(1441, 20, '443', 'Santa María Yavesía', 1),
(1442, 20, '444', 'Santa María Yolotepec', 1),
(1443, 20, '445', 'Santa María Yosoyúa', 1),
(1444, 20, '446', 'Santa María Yucuhiti', 1),
(1445, 20, '447', 'Santa María Zacatepec', 1),
(1446, 20, '448', 'Santa María Zaniza', 1),
(1447, 20, '449', 'Santa María Zoquitlán', 1),
(1448, 20, '450', 'Santiago Amoltepec', 1),
(1449, 20, '451', 'Santiago Apoala', 1),
(1450, 20, '452', 'Santiago Apóstol', 1),
(1451, 20, '453', 'Santiago Astata', 1),
(1452, 20, '454', 'Santiago Atitlán', 1),
(1453, 20, '455', 'Santiago Ayuquililla', 1),
(1454, 20, '456', 'Santiago Cacaloxtepec', 1),
(1455, 20, '457', 'Santiago Camotlán', 1),
(1456, 20, '458', 'Santiago Comaltepec', 1),
(1457, 20, '459', 'Santiago Chazumba', 1),
(1458, 20, '460', 'Santiago Choápam', 1),
(1459, 20, '461', 'Santiago del Río', 1),
(1460, 20, '462', 'Santiago Huajolotitlán', 1),
(1461, 20, '463', 'Santiago Huauclilla', 1),
(1462, 20, '464', 'Santiago Ihuitlán Plumas', 1),
(1463, 20, '465', 'Santiago Ixcuintepec', 1),
(1464, 20, '466', 'Santiago Ixtayutla', 1),
(1465, 20, '467', 'Santiago Jamiltepec', 1),
(1466, 20, '468', 'Santiago Jocotepec', 1),
(1467, 20, '469', 'Santiago Juxtlahuaca', 1),
(1468, 20, '470', 'Santiago Lachiguiri', 1),
(1469, 20, '471', 'Santiago Lalopa', 1),
(1470, 20, '472', 'Santiago Laollaga', 1),
(1471, 20, '473', 'Santiago Laxopa', 1),
(1472, 20, '474', 'Santiago Llano Grande', 1),
(1473, 20, '475', 'Santiago Matatlán', 1),
(1474, 20, '476', 'Santiago Miltepec', 1),
(1475, 20, '477', 'Santiago Minas', 1),
(1476, 20, '478', 'Santiago Nacaltepec', 1),
(1477, 20, '479', 'Santiago Nejapilla', 1),
(1478, 20, '480', 'Santiago Nundiche', 1),
(1479, 20, '481', 'Santiago Nuyoó', 1),
(1480, 20, '482', 'Santiago Pinotepa Nacional', 1),
(1481, 20, '483', 'Santiago Suchilquitongo', 1),
(1482, 20, '484', 'Santiago Tamazola', 1),
(1483, 20, '485', 'Santiago Tapextla', 1),
(1484, 20, '486', 'Villa Tejúpam de la Unión', 1),
(1485, 20, '487', 'Santiago Tenango', 1),
(1486, 20, '488', 'Santiago Tepetlapa', 1),
(1487, 20, '489', 'Santiago Tetepec', 1),
(1488, 20, '490', 'Santiago Texcalcingo', 1),
(1489, 20, '491', 'Santiago Textitlán', 1),
(1490, 20, '492', 'Santiago Tilantongo', 1),
(1491, 20, '493', 'Santiago Tillo', 1),
(1492, 20, '494', 'Santiago Tlazoyaltepec', 1),
(1493, 20, '495', 'Santiago Xanica', 1),
(1494, 20, '496', 'Santiago Xiacuí', 1),
(1495, 20, '497', 'Santiago Yaitepec', 1),
(1496, 20, '498', 'Santiago Yaveo', 1),
(1497, 20, '499', 'Santiago Yolomécatl', 1),
(1498, 20, '500', 'Santiago Yosondúa', 1),
(1499, 20, '501', 'Santiago Yucuyachi', 1),
(1500, 20, '502', 'Santiago Zacatepec', 1),
(1501, 20, '503', 'Santiago Zoochila', 1),
(1502, 20, '504', 'Nuevo Zoquiápam', 1),
(1503, 20, '505', 'Santo Domingo Ingenio', 1),
(1504, 20, '506', 'Santo Domingo Albarradas', 1),
(1505, 20, '507', 'Santo Domingo Armenta', 1),
(1506, 20, '508', 'Santo Domingo Chihuitán', 1),
(1507, 20, '509', 'Santo Domingo de Morelos', 1),
(1508, 20, '510', 'Santo Domingo Ixcatlán', 1),
(1509, 20, '511', 'Santo Domingo Nuxaá', 1),
(1510, 20, '512', 'Santo Domingo Ozolotepec', 1),
(1511, 20, '513', 'Santo Domingo Petapa', 1),
(1512, 20, '514', 'Santo Domingo Roayaga', 1),
(1513, 20, '515', 'Santo Domingo Tehuantepec', 1),
(1514, 20, '516', 'Santo Domingo Teojomulco', 1),
(1515, 20, '517', 'Santo Domingo Tepuxtepec', 1),
(1516, 20, '518', 'Santo Domingo Tlatayápam', 1),
(1517, 20, '519', 'Santo Domingo Tomaltepec', 1),
(1518, 20, '520', 'Santo Domingo Tonalá', 1),
(1519, 20, '521', 'Santo Domingo Tonaltepec', 1),
(1520, 20, '522', 'Santo Domingo Xagacía', 1),
(1521, 20, '523', 'Santo Domingo Yanhuitlán', 1),
(1522, 20, '524', 'Santo Domingo Yodohino', 1),
(1523, 20, '525', 'Santo Domingo Zanatepec', 1),
(1524, 20, '526', 'Santos Reyes Nopala', 1),
(1525, 20, '527', 'Santos Reyes Pápalo', 1),
(1526, 20, '528', 'Santos Reyes Tepejillo', 1),
(1527, 20, '529', 'Santos Reyes Yucuná', 1),
(1528, 20, '530', 'Santo Tomás Jalieza', 1),
(1529, 20, '531', 'Santo Tomás Mazaltepec', 1),
(1530, 20, '532', 'Santo Tomás Ocotepec', 1),
(1531, 20, '533', 'Santo Tomás Tamazulapan', 1),
(1532, 20, '534', 'San Vicente Coatlán', 1),
(1533, 20, '535', 'San Vicente Lachixío', 1),
(1534, 20, '536', 'San Vicente Nuñú', 1),
(1535, 20, '537', 'Silacayoápam', 1),
(1536, 20, '538', 'Sitio de Xitlapehua', 1),
(1537, 20, '539', 'Soledad Etla', 1),
(1538, 20, '540', 'Villa de Tamazulápam del Progreso', 1),
(1539, 20, '541', 'Tanetze de Zaragoza', 1),
(1540, 20, '542', 'Taniche', 1),
(1541, 20, '543', 'Tataltepec de Valdés', 1),
(1542, 20, '544', 'Teococuilco de Marcos Pérez', 1),
(1543, 20, '545', 'Teotitlán de Flores Magón', 1),
(1544, 20, '546', 'Teotitlán del Valle', 1),
(1545, 20, '547', 'Teotongo', 1),
(1546, 20, '548', 'Tepelmeme Villa de Morelos', 1),
(1547, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1548, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1549, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1550, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1551, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1552, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1553, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1554, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1555, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1556, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1557, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1558, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1559, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1560, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1561, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1562, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1563, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1564, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1565, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1566, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1567, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1568, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1569, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1570, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1571, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1572, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1573, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1574, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1575, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1576, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1577, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1578, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1579, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1580, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1581, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1582, 20, '549', 'Heroica Villa Tezoatlán de Segura y Luna, Cuna de ', 1),
(1583, 20, '550', 'San Jerónimo Tlacochahuaya', 1),
(1584, 20, '551', 'Tlacolula de Matamoros', 1),
(1585, 20, '552', 'Tlacotepec Plumas', 1),
(1586, 20, '553', 'Tlalixtac de Cabrera', 1),
(1587, 20, '554', 'Totontepec Villa de Morelos', 1),
(1588, 20, '555', 'Trinidad Zaachila', 1),
(1589, 20, '556', 'La Trinidad Vista Hermosa', 1),
(1590, 20, '557', 'Unión Hidalgo', 1),
(1591, 20, '558', 'Valerio Trujano', 1),
(1592, 20, '559', 'San Juan Bautista Valle Nacional', 1),
(1593, 20, '560', 'Villa Díaz Ordaz', 1),
(1594, 20, '561', 'Yaxe', 1),
(1595, 20, '562', 'Magdalena Yodocono de Porfirio Díaz', 1),
(1596, 20, '563', 'Yogana', 1),
(1597, 20, '564', 'Yutanduchi de Guerrero', 1),
(1598, 20, '565', 'Villa de Zaachila', 1),
(1599, 20, '566', 'San Mateo Yucutindoo', 1),
(1600, 20, '567', 'Zapotitlán Lagunas', 1),
(1601, 20, '568', 'Zapotitlán Palmas', 1),
(1602, 20, '569', 'Santa Inés de Zaragoza', 1),
(1603, 20, '570', 'Zimatlán de Álvarez', 1),
(1604, 21, '001', 'Acajete', 1),
(1605, 21, '002', 'Acateno', 1),
(1606, 21, '003', 'Acatlán', 1),
(1607, 21, '004', 'Acatzingo', 1),
(1608, 21, '005', 'Acteopan', 1),
(1609, 21, '006', 'Ahuacatlán', 1),
(1610, 21, '007', 'Ahuatlán', 1),
(1611, 21, '008', 'Ahuazotepec', 1),
(1612, 21, '009', 'Ahuehuetitla', 1),
(1613, 21, '010', 'Ajalpan', 1),
(1614, 21, '011', 'Albino Zertuche', 1),
(1615, 21, '012', 'Aljojuca', 1),
(1616, 21, '013', 'Altepexi', 1),
(1617, 21, '014', 'Amixtlán', 1),
(1618, 21, '015', 'Amozoc', 1),
(1619, 21, '016', 'Aquixtla', 1),
(1620, 21, '017', 'Atempan', 1),
(1621, 21, '018', 'Atexcal', 1),
(1622, 21, '019', 'Atlixco', 1),
(1623, 21, '020', 'Atoyatempan', 1),
(1624, 21, '021', 'Atzala', 1),
(1625, 21, '022', 'Atzitzihuacán', 1),
(1626, 21, '023', 'Atzitzintla', 1),
(1627, 21, '024', 'Axutla', 1),
(1628, 21, '025', 'Ayotoxco de Guerrero', 1),
(1629, 21, '026', 'Calpan', 1),
(1630, 21, '027', 'Caltepec', 1),
(1631, 21, '028', 'Camocuautla', 1),
(1632, 21, '029', 'Caxhuacan', 1),
(1633, 21, '030', 'Coatepec', 1),
(1634, 21, '031', 'Coatzingo', 1),
(1635, 21, '032', 'Cohetzala', 1),
(1636, 21, '033', 'Cohuecan', 1),
(1637, 21, '034', 'Coronango', 1),
(1638, 21, '035', 'Coxcatlán', 1),
(1639, 21, '036', 'Coyomeapan', 1),
(1640, 21, '037', 'Coyotepec', 1),
(1641, 21, '038', 'Cuapiaxtla de Madero', 1),
(1642, 21, '039', 'Cuautempan', 1),
(1643, 21, '040', 'Cuautinchán', 1),
(1644, 21, '041', 'Cuautlancingo', 1),
(1645, 21, '042', 'Cuayuca de Andrade', 1),
(1646, 21, '043', 'Cuetzalan del Progreso', 1),
(1647, 21, '044', 'Cuyoaco', 1),
(1648, 21, '045', 'Chalchicomula de Sesma', 1),
(1649, 21, '046', 'Chapulco', 1),
(1650, 21, '047', 'Chiautla', 1),
(1651, 21, '048', 'Chiautzingo', 1),
(1652, 21, '049', 'Chiconcuautla', 1),
(1653, 21, '050', 'Chichiquila', 1),
(1654, 21, '051', 'Chietla', 1),
(1655, 21, '052', 'Chigmecatitlán', 1),
(1656, 21, '053', 'Chignahuapan', 1),
(1657, 21, '054', 'Chignautla', 1),
(1658, 21, '055', 'Chila', 1),
(1659, 21, '056', 'Chila de la Sal', 1),
(1660, 21, '057', 'Honey', 1),
(1661, 21, '058', 'Chilchotla', 1),
(1662, 21, '059', 'Chinantla', 1),
(1663, 21, '060', 'Domingo Arenas', 1),
(1664, 21, '061', 'Eloxochitlán', 1),
(1665, 21, '062', 'Epatlán', 1),
(1666, 21, '063', 'Esperanza', 1),
(1667, 21, '064', 'Francisco Z. Mena', 1),
(1668, 21, '065', 'General Felipe Ángeles', 1),
(1669, 21, '066', 'Guadalupe', 1),
(1670, 21, '067', 'Guadalupe Victoria', 1),
(1671, 21, '068', 'Hermenegildo Galeana', 1),
(1672, 21, '069', 'Huaquechula', 1),
(1673, 21, '070', 'Huatlatlauca', 1),
(1674, 21, '071', 'Huauchinango', 1),
(1675, 21, '072', 'Huehuetla', 1),
(1676, 21, '073', 'Huehuetlán el Chico', 1),
(1677, 21, '074', 'Huejotzingo', 1),
(1678, 21, '075', 'Hueyapan', 1),
(1679, 21, '076', 'Hueytamalco', 1),
(1680, 21, '077', 'Hueytlalpan', 1),
(1681, 21, '078', 'Huitzilan de Serdán', 1),
(1682, 21, '079', 'Huitziltepec', 1),
(1683, 21, '080', 'Atlequizayan', 1),
(1684, 21, '081', 'Ixcamilpa de Guerrero', 1),
(1685, 21, '082', 'Ixcaquixtla', 1),
(1686, 21, '083', 'Ixtacamaxtitlán', 1),
(1687, 21, '084', 'Ixtepec', 1),
(1688, 21, '085', 'Izúcar de Matamoros', 1),
(1689, 21, '086', 'Jalpan', 1),
(1690, 21, '087', 'Jolalpan', 1),
(1691, 21, '088', 'Jonotla', 1),
(1692, 21, '089', 'Jopala', 1),
(1693, 21, '090', 'Juan C. Bonilla', 1),
(1694, 21, '091', 'Juan Galindo', 1),
(1695, 21, '092', 'Juan N. Méndez', 1),
(1696, 21, '093', 'Lafragua', 1),
(1697, 21, '094', 'Libres', 1),
(1698, 21, '095', 'La Magdalena Tlatlauquitepec', 1),
(1699, 21, '096', 'Mazapiltepec de Juárez', 1),
(1700, 21, '097', 'Mixtla', 1),
(1701, 21, '098', 'Molcaxac', 1),
(1702, 21, '099', 'Cañada Morelos', 1),
(1703, 21, '100', 'Naupan', 1),
(1704, 21, '101', 'Nauzontla', 1),
(1705, 21, '102', 'Nealtican', 1),
(1706, 21, '103', 'Nicolás Bravo', 1),
(1707, 21, '104', 'Nopalucan', 1),
(1708, 21, '105', 'Ocotepec', 1),
(1709, 21, '106', 'Ocoyucan', 1),
(1710, 21, '107', 'Olintla', 1),
(1711, 21, '108', 'Oriental', 1),
(1712, 21, '109', 'Pahuatlán', 1),
(1713, 21, '110', 'Palmar de Bravo', 1),
(1714, 21, '111', 'Pantepec', 1),
(1715, 21, '112', 'Petlalcingo', 1),
(1716, 21, '113', 'Piaxtla', 1),
(1717, 21, '114', 'Puebla', 1),
(1718, 21, '115', 'Quecholac', 1),
(1719, 21, '116', 'Quimixtlán', 1),
(1720, 21, '117', 'Rafael Lara Grajales', 1),
(1721, 21, '118', 'Los Reyes de Juárez', 1),
(1722, 21, '119', 'San Andrés Cholula', 1),
(1723, 21, '120', 'San Antonio Cañada', 1),
(1724, 21, '121', 'San Diego la Mesa Tochimiltzingo', 1),
(1725, 21, '122', 'San Felipe Teotlalcingo', 1),
(1726, 21, '123', 'San Felipe Tepatlán', 1),
(1727, 21, '124', 'San Gabriel Chilac', 1),
(1728, 21, '125', 'San Gregorio Atzompa', 1),
(1729, 21, '126', 'San Jerónimo Tecuanipan', 1),
(1730, 21, '127', 'San Jerónimo Xayacatlán', 1),
(1731, 21, '128', 'San José Chiapa', 1),
(1732, 21, '129', 'San José Miahuatlán', 1),
(1733, 21, '130', 'San Juan Atenco', 1),
(1734, 21, '131', 'San Juan Atzompa', 1),
(1735, 21, '132', 'San Martín Texmelucan', 1),
(1736, 21, '133', 'San Martín Totoltepec', 1),
(1737, 21, '134', 'San Matías Tlalancaleca', 1),
(1738, 21, '135', 'San Miguel Ixitlán', 1),
(1739, 21, '136', 'San Miguel Xoxtla', 1),
(1740, 21, '137', 'San Nicolás Buenos Aires', 1),
(1741, 21, '138', 'San Nicolás de los Ranchos', 1),
(1742, 21, '139', 'San Pablo Anicano', 1),
(1743, 21, '140', 'San Pedro Cholula', 1),
(1744, 21, '141', 'San Pedro Yeloixtlahuaca', 1),
(1745, 21, '142', 'San Salvador el Seco', 1),
(1746, 21, '143', 'San Salvador el Verde', 1),
(1747, 21, '144', 'San Salvador Huixcolotla', 1),
(1748, 21, '145', 'San Sebastián Tlacotepec', 1),
(1749, 21, '146', 'Santa Catarina Tlaltempan', 1),
(1750, 21, '147', 'Santa Inés Ahuatempan', 1),
(1751, 21, '148', 'Santa Isabel Cholula', 1),
(1752, 21, '149', 'Santiago Miahuatlán', 1),
(1753, 21, '150', 'Huehuetlán el Grande', 1),
(1754, 21, '151', 'Santo Tomás Hueyotlipan', 1),
(1755, 21, '152', 'Soltepec', 1),
(1756, 21, '153', 'Tecali de Herrera', 1),
(1757, 21, '154', 'Tecamachalco', 1),
(1758, 21, '155', 'Tecomatlán', 1),
(1759, 21, '156', 'Tehuacán', 1),
(1760, 21, '157', 'Tehuitzingo', 1),
(1761, 21, '158', 'Tenampulco', 1),
(1762, 21, '159', 'Teopantlán', 1),
(1763, 21, '160', 'Teotlalco', 1),
(1764, 21, '161', 'Tepanco de López', 1),
(1765, 21, '162', 'Tepango de Rodríguez', 1),
(1766, 21, '163', 'Tepatlaxco de Hidalgo', 1),
(1767, 21, '164', 'Tepeaca', 1),
(1768, 21, '165', 'Tepemaxalco', 1),
(1769, 21, '166', 'Tepeojuma', 1),
(1770, 21, '167', 'Tepetzintla', 1),
(1771, 21, '168', 'Tepexco', 1),
(1772, 21, '169', 'Tepexi de Rodríguez', 1),
(1773, 21, '170', 'Tepeyahualco', 1),
(1774, 21, '171', 'Tepeyahualco de Cuauhtémoc', 1),
(1775, 21, '172', 'Tetela de Ocampo', 1),
(1776, 21, '173', 'Teteles de Avila Castillo', 1),
(1777, 21, '174', 'Teziutlán', 1),
(1778, 21, '175', 'Tianguismanalco', 1),
(1779, 21, '176', 'Tilapa', 1),
(1780, 21, '177', 'Tlacotepec de Benito Juárez', 1),
(1781, 21, '178', 'Tlacuilotepec', 1),
(1782, 21, '179', 'Tlachichuca', 1),
(1783, 21, '180', 'Tlahuapan', 1),
(1784, 21, '181', 'Tlaltenango', 1),
(1785, 21, '182', 'Tlanepantla', 1),
(1786, 21, '183', 'Tlaola', 1),
(1787, 21, '184', 'Tlapacoya', 1),
(1788, 21, '185', 'Tlapanalá', 1),
(1789, 21, '186', 'Tlatlauquitepec', 1),
(1790, 21, '187', 'Tlaxco', 1),
(1791, 21, '188', 'Tochimilco', 1),
(1792, 21, '189', 'Tochtepec', 1),
(1793, 21, '190', 'Totoltepec de Guerrero', 1),
(1794, 21, '191', 'Tulcingo', 1),
(1795, 21, '192', 'Tuzamapan de Galeana', 1),
(1796, 21, '193', 'Tzicatlacoyan', 1),
(1797, 21, '194', 'Venustiano Carranza', 1),
(1798, 21, '195', 'Vicente Guerrero', 1),
(1799, 21, '196', 'Xayacatlán de Bravo', 1),
(1800, 21, '197', 'Xicotepec', 1),
(1801, 21, '198', 'Xicotlán', 1),
(1802, 21, '199', 'Xiutetelco', 1),
(1803, 21, '200', 'Xochiapulco', 1),
(1804, 21, '201', 'Xochiltepec', 1),
(1805, 21, '202', 'Xochitlán de Vicente Suárez', 1),
(1806, 21, '203', 'Xochitlán Todos Santos', 1),
(1807, 21, '204', 'Yaonáhuac', 1),
(1808, 21, '205', 'Yehualtepec', 1),
(1809, 21, '206', 'Zacapala', 1),
(1810, 21, '207', 'Zacapoaxtla', 1),
(1811, 21, '208', 'Zacatlán', 1),
(1812, 21, '209', 'Zapotitlán', 1),
(1813, 21, '210', 'Zapotitlán de Méndez', 1),
(1814, 21, '211', 'Zaragoza', 1),
(1815, 21, '212', 'Zautla', 1),
(1816, 21, '213', 'Zihuateutla', 1),
(1817, 21, '214', 'Zinacatepec', 1),
(1818, 21, '215', 'Zongozotla', 1),
(1819, 21, '216', 'Zoquiapan', 1),
(1820, 21, '217', 'Zoquitlán', 1),
(1821, 22, '001', 'Amealco de Bonfil', 1),
(1822, 22, '002', 'Pinal de Amoles', 1),
(1823, 22, '003', 'Arroyo Seco', 1),
(1824, 22, '004', 'Cadereyta de Montes', 1),
(1825, 22, '005', 'Colón', 1),
(1826, 22, '006', 'Corregidora', 1),
(1827, 22, '007', 'Ezequiel Montes', 1),
(1828, 22, '008', 'Huimilpan', 1),
(1829, 22, '009', 'Jalpan de Serra', 1),
(1830, 22, '010', 'Landa de Matamoros', 1),
(1831, 22, '011', 'El Marqués', 1),
(1832, 22, '012', 'Pedro Escobedo', 1),
(1833, 22, '013', 'Peñamiller', 1),
(1834, 22, '014', 'Querétaro', 1),
(1835, 22, '015', 'San Joaquín', 1),
(1836, 22, '016', 'San Juan del Río', 1),
(1837, 22, '017', 'Tequisquiapan', 1),
(1838, 22, '018', 'Tolimán', 1),
(1839, 23, '001', 'Cozumel', 1),
(1840, 23, '002', 'Felipe Carrillo Puerto', 1),
(1841, 23, '003', 'Isla Mujeres', 1),
(1842, 23, '004', 'Othón P. Blanco', 1),
(1843, 23, '005', 'Benito Juárez', 1),
(1844, 23, '006', 'José María Morelos', 1),
(1845, 23, '007', 'Lázaro Cárdenas', 1),
(1846, 23, '008', 'Solidaridad', 1),
(1847, 23, '009', 'Tulum', 1),
(1848, 23, '010', 'Bacalar', 1),
(1849, 24, '001', 'Ahualulco', 1),
(1850, 24, '002', 'Alaquines', 1),
(1851, 24, '003', 'Aquismón', 1),
(1852, 24, '004', 'Armadillo de los Infante', 1),
(1853, 24, '005', 'Cárdenas', 1),
(1854, 24, '006', 'Catorce', 1),
(1855, 24, '007', 'Cedral', 1),
(1856, 24, '008', 'Cerritos', 1),
(1857, 24, '009', 'Cerro de San Pedro', 1),
(1858, 24, '010', 'Ciudad del Maíz', 1),
(1859, 24, '011', 'Ciudad Fernández', 1),
(1860, 24, '012', 'Tancanhuitz', 1),
(1861, 24, '013', 'Ciudad Valles', 1),
(1862, 24, '014', 'Coxcatlán', 1),
(1863, 24, '015', 'Charcas', 1),
(1864, 24, '016', 'Ebano', 1),
(1865, 24, '017', 'Guadalcázar', 1),
(1866, 24, '018', 'Huehuetlán', 1),
(1867, 24, '019', 'Lagunillas', 1),
(1868, 24, '020', 'Matehuala', 1),
(1869, 24, '021', 'Mexquitic de Carmona', 1),
(1870, 24, '022', 'Moctezuma', 1),
(1871, 24, '023', 'Rayón', 1),
(1872, 24, '024', 'Rioverde', 1),
(1873, 24, '025', 'Salinas', 1),
(1874, 24, '026', 'San Antonio', 1),
(1875, 24, '027', 'San Ciro de Acosta', 1),
(1876, 24, '028', 'San Luis Potosí', 1),
(1877, 24, '029', 'San Martín Chalchicuautla', 1),
(1878, 24, '030', 'San Nicolás Tolentino', 1),
(1879, 24, '031', 'Santa Catarina', 1),
(1880, 24, '032', 'Santa María del Río', 1),
(1881, 24, '033', 'Santo Domingo', 1),
(1882, 24, '034', 'San Vicente Tancuayalab', 1),
(1883, 24, '035', 'Soledad de Graciano Sánchez', 1),
(1884, 24, '036', 'Tamasopo', 1),
(1885, 24, '037', 'Tamazunchale', 1),
(1886, 24, '038', 'Tampacán', 1),
(1887, 24, '039', 'Tampamolón Corona', 1),
(1888, 24, '040', 'Tamuín', 1),
(1889, 24, '041', 'Tanlajás', 1),
(1890, 24, '042', 'Tanquián de Escobedo', 1),
(1891, 24, '043', 'Tierra Nueva', 1),
(1892, 24, '044', 'Vanegas', 1),
(1893, 24, '045', 'Venado', 1),
(1894, 24, '046', 'Villa de Arriaga', 1),
(1895, 24, '047', 'Villa de Guadalupe', 1),
(1896, 24, '048', 'Villa de la Paz', 1),
(1897, 24, '049', 'Villa de Ramos', 1),
(1898, 24, '050', 'Villa de Reyes', 1),
(1899, 24, '051', 'Villa Hidalgo', 1),
(1900, 24, '052', 'Villa Juárez', 1),
(1901, 24, '053', 'Axtla de Terrazas', 1),
(1902, 24, '054', 'Xilitla', 1),
(1903, 24, '055', 'Zaragoza', 1),
(1904, 24, '056', 'Villa de Arista', 1),
(1905, 24, '057', 'Matlapa', 1),
(1906, 24, '058', 'El Naranjo', 1),
(1907, 25, '001', 'Ahome', 1),
(1908, 25, '002', 'Angostura', 1),
(1909, 25, '003', 'Badiraguato', 1),
(1910, 25, '004', 'Concordia', 1),
(1911, 25, '005', 'Cosalá', 1),
(1912, 25, '006', 'Culiacán', 1),
(1913, 25, '007', 'Choix', 1),
(1914, 25, '008', 'Elota', 1),
(1915, 25, '009', 'Escuinapa', 1),
(1916, 25, '010', 'El Fuerte', 1),
(1917, 25, '011', 'Guasave', 1),
(1918, 25, '012', 'Mazatlán', 1),
(1919, 25, '013', 'Mocorito', 1),
(1920, 25, '014', 'Rosario', 1),
(1921, 25, '015', 'Salvador Alvarado', 1),
(1922, 25, '016', 'San Ignacio', 1),
(1923, 25, '017', 'Sinaloa', 1),
(1924, 25, '018', 'Navolato', 1),
(1925, 26, '001', 'Aconchi', 1),
(1926, 26, '002', 'Agua Prieta', 1),
(1927, 26, '003', 'Alamos', 1),
(1928, 26, '004', 'Altar', 1),
(1929, 26, '005', 'Arivechi', 1),
(1930, 26, '006', 'Arizpe', 1),
(1931, 26, '007', 'Atil', 1),
(1932, 26, '008', 'Bacadéhuachi', 1),
(1933, 26, '009', 'Bacanora', 1),
(1934, 26, '010', 'Bacerac', 1),
(1935, 26, '011', 'Bacoachi', 1),
(1936, 26, '012', 'Bácum', 1),
(1937, 26, '013', 'Banámichi', 1),
(1938, 26, '014', 'Baviácora', 1),
(1939, 26, '015', 'Bavispe', 1),
(1940, 26, '016', 'Benjamín Hill', 1),
(1941, 26, '017', 'Caborca', 1),
(1942, 26, '018', 'Cajeme', 1),
(1943, 26, '019', 'Cananea', 1),
(1944, 26, '020', 'Carbó', 1),
(1945, 26, '021', 'La Colorada', 1),
(1946, 26, '022', 'Cucurpe', 1),
(1947, 26, '023', 'Cumpas', 1),
(1948, 26, '024', 'Divisaderos', 1),
(1949, 26, '025', 'Empalme', 1),
(1950, 26, '026', 'Etchojoa', 1),
(1951, 26, '027', 'Fronteras', 1),
(1952, 26, '028', 'Granados', 1),
(1953, 26, '029', 'Guaymas', 1),
(1954, 26, '030', 'Hermosillo', 1),
(1955, 26, '031', 'Huachinera', 1),
(1956, 26, '032', 'Huásabas', 1),
(1957, 26, '033', 'Huatabampo', 1),
(1958, 26, '034', 'Huépac', 1),
(1959, 26, '035', 'Imuris', 1),
(1960, 26, '036', 'Magdalena', 1),
(1961, 26, '037', 'Mazatán', 1),
(1962, 26, '038', 'Moctezuma', 1),
(1963, 26, '039', 'Naco', 1),
(1964, 26, '040', 'Nácori Chico', 1),
(1965, 26, '041', 'Nacozari de García', 1),
(1966, 26, '042', 'Navojoa', 1),
(1967, 26, '043', 'Nogales', 1),
(1968, 26, '044', 'Onavas', 1),
(1969, 26, '045', 'Opodepe', 1),
(1970, 26, '046', 'Oquitoa', 1),
(1971, 26, '047', 'Pitiquito', 1),
(1972, 26, '048', 'Puerto Peñasco', 1),
(1973, 26, '049', 'Quiriego', 1),
(1974, 26, '050', 'Rayón', 1),
(1975, 26, '051', 'Rosario', 1),
(1976, 26, '052', 'Sahuaripa', 1),
(1977, 26, '053', 'San Felipe de Jesús', 1),
(1978, 26, '054', 'San Javier', 1),
(1979, 26, '055', 'San Luis Río Colorado', 1),
(1980, 26, '056', 'San Miguel de Horcasitas', 1),
(1981, 26, '057', 'San Pedro de la Cueva', 1),
(1982, 26, '058', 'Santa Ana', 1),
(1983, 26, '059', 'Santa Cruz', 1),
(1984, 26, '060', 'Sáric', 1),
(1985, 26, '061', 'Soyopa', 1),
(1986, 26, '062', 'Suaqui Grande', 1),
(1987, 26, '063', 'Tepache', 1),
(1988, 26, '064', 'Trincheras', 1),
(1989, 26, '065', 'Tubutama', 1),
(1990, 26, '066', 'Ures', 1),
(1991, 26, '067', 'Villa Hidalgo', 1),
(1992, 26, '068', 'Villa Pesqueira', 1),
(1993, 26, '069', 'Yécora', 1),
(1994, 26, '070', 'General Plutarco Elías Calles', 1),
(1995, 26, '071', 'Benito Juárez', 1),
(1996, 26, '072', 'San Ignacio Río Muerto', 1),
(1997, 27, '001', 'Balancán', 1),
(1998, 27, '002', 'Cárdenas', 1),
(1999, 27, '003', 'Centla', 1)

INSERT INTO Config.City(CityId,StateId,Code,Name,IsActive) VALUES
(2000, 27, '004', 'Centro', 1),
(2001, 27, '005', 'Comalcalco', 1),
(2002, 27, '006', 'Cunduacán', 1),
(2003, 27, '007', 'Emiliano Zapata', 1),
(2004, 27, '008', 'Huimanguillo', 1),
(2005, 27, '009', 'Jalapa', 1),
(2006, 27, '010', 'Jalpa de Méndez', 1),
(2007, 27, '011', 'Jonuta', 1),
(2008, 27, '012', 'Macuspana', 1),
(2009, 27, '013', 'Nacajuca', 1),
(2010, 27, '014', 'Paraíso', 1),
(2011, 27, '015', 'Tacotalpa', 1),
(2012, 27, '016', 'Teapa', 1),
(2013, 27, '017', 'Tenosique', 1),
(2014, 28, '001', 'Abasolo', 1),
(2015, 28, '002', 'Aldama', 1),
(2016, 28, '003', 'Altamira', 1),
(2017, 28, '004', 'Antiguo Morelos', 1),
(2018, 28, '005', 'Burgos', 1),
(2019, 28, '006', 'Bustamante', 1),
(2020, 28, '007', 'Camargo', 1),
(2021, 28, '008', 'Casas', 1),
(2022, 28, '009', 'Ciudad Madero', 1),
(2023, 28, '010', 'Cruillas', 1),
(2024, 28, '011', 'Gómez Farías', 1),
(2025, 28, '012', 'González', 1),
(2026, 28, '013', 'Güémez', 1),
(2027, 28, '014', 'Guerrero', 1),
(2028, 28, '015', 'Gustavo Díaz Ordaz', 1),
(2029, 28, '016', 'Hidalgo', 1),
(2030, 28, '017', 'Jaumave', 1),
(2031, 28, '018', 'Jiménez', 1),
(2032, 28, '019', 'Llera', 1),
(2033, 28, '020', 'Mainero', 1),
(2034, 28, '021', 'El Mante', 1),
(2035, 28, '022', 'Matamoros', 1),
(2036, 28, '023', 'Méndez', 1),
(2037, 28, '024', 'Mier', 1),
(2038, 28, '025', 'Miguel Alemán', 1),
(2039, 28, '026', 'Miquihuana', 1),
(2040, 28, '027', 'Nuevo Laredo', 1),
(2041, 28, '028', 'Nuevo Morelos', 1),
(2042, 28, '029', 'Ocampo', 1),
(2043, 28, '030', 'Padilla', 1),
(2044, 28, '031', 'Palmillas', 1),
(2045, 28, '032', 'Reynosa', 1),
(2046, 28, '033', 'Río Bravo', 1),
(2047, 28, '034', 'San Carlos', 1),
(2048, 28, '035', 'San Fernando', 1),
(2049, 28, '036', 'San Nicolás', 1),
(2050, 28, '037', 'Soto la Marina', 1),
(2051, 28, '038', 'Tampico', 1),
(2052, 28, '039', 'Tula', 1),
(2053, 28, '040', 'Valle Hermoso', 1),
(2054, 28, '041', 'Victoria', 1),
(2055, 28, '042', 'Villagrán', 1),
(2056, 28, '043', 'Xicoténcatl', 1),
(2057, 29, '001', 'Amaxac de Guerrero', 1),
(2058, 29, '002', 'Apetatitlán de Antonio Carvajal', 1),
(2059, 29, '003', 'Atlangatepec', 1),
(2060, 29, '004', 'Atltzayanca', 1),
(2061, 29, '005', 'Apizaco', 1),
(2062, 29, '006', 'Calpulalpan', 1),
(2063, 29, '007', 'El Carmen Tequexquitla', 1),
(2064, 29, '008', 'Cuapiaxtla', 1),
(2065, 29, '009', 'Cuaxomulco', 1),
(2066, 29, '010', 'Chiautempan', 1),
(2067, 29, '011', 'Muñoz de Domingo Arenas', 1),
(2068, 29, '012', 'Españita', 1),
(2069, 29, '013', 'Huamantla', 1),
(2070, 29, '014', 'Hueyotlipan', 1),
(2071, 29, '015', 'Ixtacuixtla de Mariano Matamoros', 1),
(2072, 29, '016', 'Ixtenco', 1),
(2073, 29, '017', 'Mazatecochco de José María Morelos', 1),
(2074, 29, '018', 'Contla de Juan Cuamatzi', 1),
(2075, 29, '019', 'Tepetitla de Lardizábal', 1),
(2076, 29, '020', 'Sanctórum de Lázaro Cárdenas', 1),
(2077, 29, '021', 'Nanacamilpa de Mariano Arista', 1),
(2078, 29, '022', 'Acuamanala de Miguel Hidalgo', 1),
(2079, 29, '023', 'Natívitas', 1),
(2080, 29, '024', 'Panotla', 1),
(2081, 29, '025', 'San Pablo del Monte', 1),
(2082, 29, '026', 'Santa Cruz Tlaxcala', 1),
(2083, 29, '027', 'Tenancingo', 1),
(2084, 29, '028', 'Teolocholco', 1),
(2085, 29, '029', 'Tepeyanco', 1),
(2086, 29, '030', 'Terrenate', 1),
(2087, 29, '031', 'Tetla de la Solidaridad', 1),
(2088, 29, '032', 'Tetlatlahuca', 1),
(2089, 29, '033', 'Tlaxcala', 1),
(2090, 29, '034', 'Tlaxco', 1),
(2091, 29, '035', 'Tocatlán', 1),
(2092, 29, '036', 'Totolac', 1),
(2093, 29, '037', 'Ziltlaltépec de Trinidad Sánchez Santos', 1),
(2094, 29, '038', 'Tzompantepec', 1),
(2095, 29, '039', 'Xaloztoc', 1),
(2096, 29, '040', 'Xaltocan', 1),
(2097, 29, '041', 'Papalotla de Xicohténcatl', 1),
(2098, 29, '042', 'Xicohtzinco', 1),
(2099, 29, '043', 'Yauhquemehcan', 1),
(2100, 29, '044', 'Zacatelco', 1),
(2101, 29, '045', 'Benito Juárez', 1),
(2102, 29, '046', 'Emiliano Zapata', 1),
(2103, 29, '047', 'Lázaro Cárdenas', 1),
(2104, 29, '048', 'La Magdalena Tlaltelulco', 1),
(2105, 29, '049', 'San Damián Texóloc', 1),
(2106, 29, '050', 'San Francisco Tetlanohcan', 1),
(2107, 29, '051', 'San Jerónimo Zacualpan', 1),
(2108, 29, '052', 'San José Teacalco', 1),
(2109, 29, '053', 'San Juan Huactzinco', 1),
(2110, 29, '054', 'San Lorenzo Axocomanitla', 1),
(2111, 29, '055', 'San Lucas Tecopilco', 1),
(2112, 29, '056', 'Santa Ana Nopalucan', 1),
(2113, 29, '057', 'Santa Apolonia Teacalco', 1),
(2114, 29, '058', 'Santa Catarina Ayometla', 1),
(2115, 29, '059', 'Santa Cruz Quilehtla', 1),
(2116, 29, '060', 'Santa Isabel Xiloxoxtla', 1),
(2117, 30, '001', 'Acajete', 1),
(2118, 30, '002', 'Acatlán', 1),
(2119, 30, '003', 'Acayucan', 1),
(2120, 30, '004', 'Actopan', 1),
(2121, 30, '005', 'Acula', 1),
(2122, 30, '006', 'Acultzingo', 1),
(2123, 30, '007', 'Camarón de Tejeda', 1),
(2124, 30, '008', 'Alpatláhuac', 1),
(2125, 30, '009', 'Alto Lucero de Gutiérrez Barrios', 1),
(2126, 30, '010', 'Altotonga', 1),
(2127, 30, '011', 'Alvarado', 1),
(2128, 30, '012', 'Amatitlán', 1),
(2129, 30, '013', 'Naranjos Amatlán', 1),
(2130, 30, '014', 'Amatlán de los Reyes', 1),
(2131, 30, '015', 'Angel R. Cabada', 1),
(2132, 30, '016', 'La Antigua', 1),
(2133, 30, '017', 'Apazapan', 1),
(2134, 30, '018', 'Aquila', 1),
(2135, 30, '019', 'Astacinga', 1),
(2136, 30, '020', 'Atlahuilco', 1),
(2137, 30, '021', 'Atoyac', 1),
(2138, 30, '022', 'Atzacan', 1),
(2139, 30, '023', 'Atzalan', 1),
(2140, 30, '024', 'Tlaltetela', 1),
(2141, 30, '025', 'Ayahualulco', 1),
(2142, 30, '026', 'Banderilla', 1),
(2143, 30, '027', 'Benito Juárez', 1),
(2144, 30, '028', 'Boca del Río', 1),
(2145, 30, '029', 'Calcahualco', 1),
(2146, 30, '030', 'Camerino Z. Mendoza', 1),
(2147, 30, '031', 'Carrillo Puerto', 1),
(2148, 30, '032', 'Catemaco', 1),
(2149, 30, '033', 'Cazones de Herrera', 1),
(2150, 30, '034', 'Cerro Azul', 1),
(2151, 30, '035', 'Citlaltépetl', 1),
(2152, 30, '036', 'Coacoatzintla', 1),
(2153, 30, '037', 'Coahuitlán', 1),
(2154, 30, '038', 'Coatepec', 1),
(2155, 30, '039', 'Coatzacoalcos', 1),
(2156, 30, '040', 'Coatzintla', 1),
(2157, 30, '041', 'Coetzala', 1),
(2158, 30, '042', 'Colipa', 1),
(2159, 30, '043', 'Comapa', 1),
(2160, 30, '044', 'Córdoba', 1),
(2161, 30, '045', 'Cosamaloapan de Carpio', 1),
(2162, 30, '046', 'Cosautlán de Carvajal', 1),
(2163, 30, '047', 'Coscomatepec', 1),
(2164, 30, '048', 'Cosoleacaque', 1),
(2165, 30, '049', 'Cotaxtla', 1),
(2166, 30, '050', 'Coxquihui', 1),
(2167, 30, '051', 'Coyutla', 1),
(2168, 30, '052', 'Cuichapa', 1),
(2169, 30, '053', 'Cuitláhuac', 1),
(2170, 30, '054', 'Chacaltianguis', 1),
(2171, 30, '055', 'Chalma', 1),
(2172, 30, '056', 'Chiconamel', 1),
(2173, 30, '057', 'Chiconquiaco', 1),
(2174, 30, '058', 'Chicontepec', 1),
(2175, 30, '059', 'Chinameca', 1),
(2176, 30, '060', 'Chinampa de Gorostiza', 1),
(2177, 30, '061', 'Las Choapas', 1),
(2178, 30, '062', 'Chocamán', 1),
(2179, 30, '063', 'Chontla', 1),
(2180, 30, '064', 'Chumatlán', 1),
(2181, 30, '065', 'Emiliano Zapata', 1),
(2182, 30, '066', 'Espinal', 1),
(2183, 30, '067', 'Filomeno Mata', 1),
(2184, 30, '068', 'Fortín', 1),
(2185, 30, '069', 'Gutiérrez Zamora', 1),
(2186, 30, '070', 'Hidalgotitlán', 1),
(2187, 30, '071', 'Huatusco', 1),
(2188, 30, '072', 'Huayacocotla', 1),
(2189, 30, '073', 'Hueyapan de Ocampo', 1),
(2190, 30, '074', 'Huiloapan de Cuauhtémoc', 1),
(2191, 30, '075', 'Ignacio de la Llave', 1),
(2192, 30, '076', 'Ilamatlán', 1),
(2193, 30, '077', 'Isla', 1),
(2194, 30, '078', 'Ixcatepec', 1),
(2195, 30, '079', 'Ixhuacán de los Reyes', 1),
(2196, 30, '080', 'Ixhuatlán del Café', 1),
(2197, 30, '081', 'Ixhuatlancillo', 1),
(2198, 30, '082', 'Ixhuatlán del Sureste', 1),
(2199, 30, '083', 'Ixhuatlán de Madero', 1),
(2200, 30, '084', 'Ixmatlahuacan', 1),
(2201, 30, '085', 'Ixtaczoquitlán', 1),
(2202, 30, '086', 'Jalacingo', 1),
(2203, 30, '087', 'Xalapa', 1),
(2204, 30, '088', 'Jalcomulco', 1),
(2205, 30, '089', 'Jáltipan', 1),
(2206, 30, '090', 'Jamapa', 1),
(2207, 30, '091', 'Jesús Carranza', 1),
(2208, 30, '092', 'Xico', 1),
(2209, 30, '093', 'Jilotepec', 1),
(2210, 30, '094', 'Juan Rodríguez Clara', 1),
(2211, 30, '095', 'Juchique de Ferrer', 1),
(2212, 30, '096', 'Landero y Coss', 1),
(2213, 30, '097', 'Lerdo de Tejada', 1),
(2214, 30, '098', 'Magdalena', 1),
(2215, 30, '099', 'Maltrata', 1),
(2216, 30, '100', 'Manlio Fabio Altamirano', 1),
(2217, 30, '101', 'Mariano Escobedo', 1),
(2218, 30, '102', 'Martínez de la Torre', 1),
(2219, 30, '103', 'Mecatlán', 1),
(2220, 30, '104', 'Mecayapan', 1),
(2221, 30, '105', 'Medellín de Bravo', 1),
(2222, 30, '106', 'Miahuatlán', 1),
(2223, 30, '107', 'Las Minas', 1),
(2224, 30, '108', 'Minatitlán', 1),
(2225, 30, '109', 'Misantla', 1),
(2226, 30, '110', 'Mixtla de Altamirano', 1),
(2227, 30, '111', 'Moloacán', 1),
(2228, 30, '112', 'Naolinco', 1),
(2229, 30, '113', 'Naranjal', 1),
(2230, 30, '114', 'Nautla', 1),
(2231, 30, '115', 'Nogales', 1),
(2232, 30, '116', 'Oluta', 1),
(2233, 30, '117', 'Omealca', 1),
(2234, 30, '118', 'Orizaba', 1),
(2235, 30, '119', 'Otatitlán', 1),
(2236, 30, '120', 'Oteapan', 1),
(2237, 30, '121', 'Ozuluama de Mascareñas', 1),
(2238, 30, '122', 'Pajapan', 1),
(2239, 30, '123', 'Pánuco', 1),
(2240, 30, '124', 'Papantla', 1),
(2241, 30, '125', 'Paso del Macho', 1),
(2242, 30, '126', 'Paso de Ovejas', 1),
(2243, 30, '127', 'La Perla', 1),
(2244, 30, '128', 'Perote', 1),
(2245, 30, '129', 'Platón Sánchez', 1),
(2246, 30, '130', 'Playa Vicente', 1),
(2247, 30, '131', 'Poza Rica de Hidalgo', 1),
(2248, 30, '132', 'Las Vigas de Ramírez', 1),
(2249, 30, '133', 'Pueblo Viejo', 1),
(2250, 30, '134', 'Puente Nacional', 1),
(2251, 30, '135', 'Rafael Delgado', 1),
(2252, 30, '136', 'Rafael Lucio', 1),
(2253, 30, '137', 'Los Reyes', 1),
(2254, 30, '138', 'Río Blanco', 1),
(2255, 30, '139', 'Saltabarranca', 1),
(2256, 30, '140', 'San Andrés Tenejapan', 1),
(2257, 30, '141', 'San Andrés Tuxtla', 1),
(2258, 30, '142', 'San Juan Evangelista', 1),
(2259, 30, '143', 'Santiago Tuxtla', 1),
(2260, 30, '144', 'Sayula de Alemán', 1),
(2261, 30, '145', 'Soconusco', 1),
(2262, 30, '146', 'Sochiapa', 1),
(2263, 30, '147', 'Soledad Atzompa', 1),
(2264, 30, '148', 'Soledad de Doblado', 1),
(2265, 30, '149', 'Soteapan', 1),
(2266, 30, '150', 'Tamalín', 1),
(2267, 30, '151', 'Tamiahua', 1),
(2268, 30, '152', 'Tampico Alto', 1),
(2269, 30, '153', 'Tancoco', 1),
(2270, 30, '154', 'Tantima', 1),
(2271, 30, '155', 'Tantoyuca', 1),
(2272, 30, '156', 'Tatatila', 1),
(2273, 30, '157', 'Castillo de Teayo', 1),
(2274, 30, '158', 'Tecolutla', 1),
(2275, 30, '159', 'Tehuipango', 1),
(2276, 30, '160', 'Álamo Temapache', 1),
(2277, 30, '161', 'Tempoal', 1),
(2278, 30, '162', 'Tenampa', 1),
(2279, 30, '163', 'Tenochtitlán', 1),
(2280, 30, '164', 'Teocelo', 1),
(2281, 30, '165', 'Tepatlaxco', 1),
(2282, 30, '166', 'Tepetlán', 1),
(2283, 30, '167', 'Tepetzintla', 1),
(2284, 30, '168', 'Tequila', 1),
(2285, 30, '169', 'José Azueta', 1),
(2286, 30, '170', 'Texcatepec', 1),
(2287, 30, '171', 'Texhuacán', 1),
(2288, 30, '172', 'Texistepec', 1),
(2289, 30, '173', 'Tezonapa', 1),
(2290, 30, '174', 'Tierra Blanca', 1),
(2291, 30, '175', 'Tihuatlán', 1),
(2292, 30, '176', 'Tlacojalpan', 1),
(2293, 30, '177', 'Tlacolulan', 1),
(2294, 30, '178', 'Tlacotalpan', 1),
(2295, 30, '179', 'Tlacotepec de Mejía', 1),
(2296, 30, '180', 'Tlachichilco', 1),
(2297, 30, '181', 'Tlalixcoyan', 1),
(2298, 30, '182', 'Tlalnelhuayocan', 1),
(2299, 30, '183', 'Tlapacoyan', 1),
(2300, 30, '184', 'Tlaquilpa', 1),
(2301, 30, '185', 'Tlilapan', 1),
(2302, 30, '186', 'Tomatlán', 1),
(2303, 30, '187', 'Tonayán', 1),
(2304, 30, '188', 'Totutla', 1),
(2305, 30, '189', 'Tuxpan', 1),
(2306, 30, '190', 'Tuxtilla', 1),
(2307, 30, '191', 'Ursulo Galván', 1),
(2308, 30, '192', 'Vega de Alatorre', 1),
(2309, 30, '193', 'Veracruz', 1),
(2310, 30, '194', 'Villa Aldama', 1),
(2311, 30, '195', 'Xoxocotla', 1),
(2312, 30, '196', 'Yanga', 1),
(2313, 30, '197', 'Yecuatla', 1),
(2314, 30, '198', 'Zacualpan', 1),
(2315, 30, '199', 'Zaragoza', 1),
(2316, 30, '200', 'Zentla', 1),
(2317, 30, '201', 'Zongolica', 1),
(2318, 30, '202', 'Zontecomatlán de López y Fuentes', 1),
(2319, 30, '203', 'Zozocolco de Hidalgo', 1),
(2320, 30, '204', 'Agua Dulce', 1),
(2321, 30, '205', 'El Higo', 1),
(2322, 30, '206', 'Nanchital de Lázaro Cárdenas del Río', 1),
(2323, 30, '207', 'Tres Valles', 1),
(2324, 30, '208', 'Carlos A. Carrillo', 1),
(2325, 30, '209', 'Tatahuicapan de Juárez', 1),
(2326, 30, '210', 'Uxpanapa', 1),
(2327, 30, '211', 'San Rafael', 1),
(2328, 30, '212', 'Santiago Sochiapan', 1),
(2329, 31, '001', 'Abalá', 1),
(2330, 31, '002', 'Acanceh', 1),
(2331, 31, '003', 'Akil', 1),
(2332, 31, '004', 'Baca', 1),
(2333, 31, '005', 'Bokobá', 1),
(2334, 31, '006', 'Buctzotz', 1),
(2335, 31, '007', 'Cacalchén', 1),
(2336, 31, '008', 'Calotmul', 1),
(2337, 31, '009', 'Cansahcab', 1),
(2338, 31, '010', 'Cantamayec', 1),
(2339, 31, '011', 'Celestún', 1),
(2340, 31, '012', 'Cenotillo', 1),
(2341, 31, '013', 'Conkal', 1),
(2342, 31, '014', 'Cuncunul', 1),
(2343, 31, '015', 'Cuzamá', 1),
(2344, 31, '016', 'Chacsinkín', 1),
(2345, 31, '017', 'Chankom', 1),
(2346, 31, '018', 'Chapab', 1),
(2347, 31, '019', 'Chemax', 1),
(2348, 31, '020', 'Chicxulub Pueblo', 1),
(2349, 31, '021', 'Chichimilá', 1),
(2350, 31, '022', 'Chikindzonot', 1),
(2351, 31, '023', 'Chocholá', 1),
(2352, 31, '024', 'Chumayel', 1),
(2353, 31, '025', 'Dzán', 1),
(2354, 31, '026', 'Dzemul', 1),
(2355, 31, '027', 'Dzidzantún', 1),
(2356, 31, '028', 'Dzilam de Bravo', 1),
(2357, 31, '029', 'Dzilam González', 1),
(2358, 31, '030', 'Dzitás', 1),
(2359, 31, '031', 'Dzoncauich', 1),
(2360, 31, '032', 'Espita', 1),
(2361, 31, '033', 'Halachó', 1),
(2362, 31, '034', 'Hocabá', 1),
(2363, 31, '035', 'Hoctún', 1),
(2364, 31, '036', 'Homún', 1),
(2365, 31, '037', 'Huhí', 1),
(2366, 31, '038', 'Hunucmá', 1),
(2367, 31, '039', 'Ixil', 1),
(2368, 31, '040', 'Izamal', 1),
(2369, 31, '041', 'Kanasín', 1),
(2370, 31, '042', 'Kantunil', 1),
(2371, 31, '043', 'Kaua', 1),
(2372, 31, '044', 'Kinchil', 1),
(2373, 31, '045', 'Kopomá', 1),
(2374, 31, '046', 'Mama', 1),
(2375, 31, '047', 'Maní', 1),
(2376, 31, '048', 'Maxcanú', 1),
(2377, 31, '049', 'Mayapán', 1),
(2378, 31, '050', 'Mérida', 1),
(2379, 31, '051', 'Mocochá', 1),
(2380, 31, '052', 'Motul', 1),
(2381, 31, '053', 'Muna', 1),
(2382, 31, '054', 'Muxupip', 1),
(2383, 31, '055', 'Opichén', 1),
(2384, 31, '056', 'Oxkutzcab', 1),
(2385, 31, '057', 'Panabá', 1),
(2386, 31, '058', 'Peto', 1),
(2387, 31, '059', 'Progreso', 1),
(2388, 31, '060', 'Quintana Roo', 1),
(2389, 31, '061', 'Río Lagartos', 1),
(2390, 31, '062', 'Sacalum', 1),
(2391, 31, '063', 'Samahil', 1),
(2392, 31, '064', 'Sanahcat', 1),
(2393, 31, '065', 'San Felipe', 1),
(2394, 31, '066', 'Santa Elena', 1),
(2395, 31, '067', 'Seyé', 1),
(2396, 31, '068', 'Sinanché', 1),
(2397, 31, '069', 'Sotuta', 1),
(2398, 31, '070', 'Sucilá', 1),
(2399, 31, '071', 'Sudzal', 1),
(2400, 31, '072', 'Suma', 1),
(2401, 31, '073', 'Tahdziú', 1),
(2402, 31, '074', 'Tahmek', 1),
(2403, 31, '075', 'Teabo', 1),
(2404, 31, '076', 'Tecoh', 1),
(2405, 31, '077', 'Tekal de Venegas', 1),
(2406, 31, '078', 'Tekantó', 1),
(2407, 31, '079', 'Tekax', 1),
(2408, 31, '080', 'Tekit', 1),
(2409, 31, '081', 'Tekom', 1),
(2410, 31, '082', 'Telchac Pueblo', 1),
(2411, 31, '083', 'Telchac Puerto', 1),
(2412, 31, '084', 'Temax', 1),
(2413, 31, '085', 'Temozón', 1),
(2414, 31, '086', 'Tepakán', 1),
(2415, 31, '087', 'Tetiz', 1),
(2416, 31, '088', 'Teya', 1),
(2417, 31, '089', 'Ticul', 1),
(2418, 31, '090', 'Timucuy', 1),
(2419, 31, '091', 'Tinum', 1),
(2420, 31, '092', 'Tixcacalcupul', 1),
(2421, 31, '093', 'Tixkokob', 1),
(2422, 31, '094', 'Tixmehuac', 1),
(2423, 31, '095', 'Tixpéhual', 1),
(2424, 31, '096', 'Tizimín', 1),
(2425, 31, '097', 'Tunkás', 1),
(2426, 31, '098', 'Tzucacab', 1),
(2427, 31, '099', 'Uayma', 1),
(2428, 31, '100', 'Ucú', 1),
(2429, 31, '101', 'Umán', 1),
(2430, 31, '102', 'Valladolid', 1),
(2431, 31, '103', 'Xocchel', 1),
(2432, 31, '104', 'Yaxcabá', 1),
(2433, 31, '105', 'Yaxkukul', 1),
(2434, 31, '106', 'Yobaín', 1),
(2435, 32, '001', 'Apozol', 1),
(2436, 32, '002', 'Apulco', 1),
(2437, 32, '003', 'Atolinga', 1),
(2438, 32, '004', 'Benito Juárez', 1),
(2439, 32, '005', 'Calera', 1),
(2440, 32, '006', 'Cañitas de Felipe Pescador', 1),
(2441, 32, '007', 'Concepción del Oro', 1),
(2442, 32, '008', 'Cuauhtémoc', 1),
(2443, 32, '009', 'Chalchihuites', 1),
(2444, 32, '010', 'Fresnillo', 1),
(2445, 32, '011', 'Trinidad García de la Cadena', 1),
(2446, 32, '012', 'Genaro Codina', 1),
(2447, 32, '013', 'General Enrique Estrada', 1),
(2448, 32, '014', 'General Francisco R. Murguía', 1),
(2449, 32, '015', 'El Plateado de Joaquín Amaro', 1),
(2450, 32, '016', 'General Pánfilo Natera', 1),
(2451, 32, '017', 'Guadalupe', 1),
(2452, 32, '018', 'Huanusco', 1),
(2453, 32, '019', 'Jalpa', 1),
(2454, 32, '020', 'Jerez', 1),
(2455, 32, '021', 'Jiménez del Teul', 1),
(2456, 32, '022', 'Juan Aldama', 1),
(2457, 32, '023', 'Juchipila', 1),
(2458, 32, '024', 'Loreto', 1),
(2459, 32, '025', 'Luis Moya', 1),
(2460, 32, '026', 'Mazapil', 1),
(2461, 32, '027', 'Melchor Ocampo', 1),
(2462, 32, '028', 'Mezquital del Oro', 1),
(2463, 32, '029', 'Miguel Auza', 1),
(2464, 32, '030', 'Momax', 1),
(2465, 32, '031', 'Monte Escobedo', 1),
(2466, 32, '032', 'Morelos', 1),
(2467, 32, '033', 'Moyahua de Estrada', 1),
(2468, 32, '034', 'Nochistlán de Mejía', 1),
(2469, 32, '035', 'Noria de Ángeles', 1),
(2470, 32, '036', 'Ojocaliente', 1),
(2471, 32, '037', 'Pánuco', 1),
(2472, 32, '038', 'Pinos', 1),
(2473, 32, '039', 'Río Grande', 1),
(2474, 32, '040', 'Sain Alto', 1),
(2475, 32, '041', 'El Salvador', 1),
(2476, 32, '042', 'Sombrerete', 1),
(2477, 32, '043', 'Susticacán', 1),
(2478, 32, '044', 'Tabasco', 1),
(2479, 32, '045', 'Tepechitlán', 1),
(2480, 32, '046', 'Tepetongo', 1),
(2481, 32, '047', 'Teúl de González Ortega', 1),
(2482, 32, '048', 'Tlaltenango de Sánchez Román', 1),
(2483, 32, '049', 'Valparaíso', 1),
(2484, 32, '050', 'Vetagrande', 1),
(2485, 32, '051', 'Villa de Cos', 1),
(2486, 32, '052', 'Villa García', 1),
(2487, 32, '053', 'Villa González Ortega', 1),
(2488, 32, '054', 'Villa Hidalgo', 1),
(2489, 32, '055', 'Villanueva', 1),
(2490, 32, '056', 'Zacatecas', 1),
(2491, 32, '057', 'Trancoso', 1),
(2492, 32, '058', 'Santa María de la Paz', 1)

SET IDENTITY_INSERT Config.City off

--================MAKES=====================================
SET IDENTITY_INSERT Config.CarMake ON

INSERT INTO Config.CarMake(CarMakeId,Name)Values(1,'Abarth')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(2,'Alfa Romeo')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(3,'Aro')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(4,'Asia')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(5,'Asia Motors')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(6,'Aston Martin')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(7,'Audi')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(8,'Austin')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(9,'Auverland')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(10,'Bentley')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(11,'Bertone')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(12,'Bmw')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(13,'Cadillac')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(14,'Chevrolet')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(15,'Chrysler')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(16,'Citroen')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(17,'Corvette')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(18,'Dacia')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(19,'Daewoo')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(20,'Daf')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(21,'Daihatsu')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(22,'Daimler')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(23,'Dodge')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(24,'Ferrari')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(25,'Fiat')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(26,'Ford')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(27,'Galloper')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(28,'Gmc')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(29,'Honda')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(30,'Hummer')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(31,'Hyundai')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(32,'Infiniti')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(33,'Innocenti')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(34,'Isuzu')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(35,'Iveco')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(36,'Iveco-pegaso')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(37,'Jaguar')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(38,'Jeep')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(39,'Kia')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(40,'Lada')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(41,'Lamborghini')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(42,'Lancia')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(43,'Land-rover')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(44,'Ldv')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(45,'Lexus')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(46,'Lotus')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(47,'Mahindra')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(48,'Maserati')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(49,'Maybach')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(50,'Mazda')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(51,'Mercedes-benz')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(52,'Mg')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(53,'Mini')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(54,'Mitsubishi')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(55,'Morgan')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(56,'Nissan')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(57,'Opel')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(58,'Peugeot')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(59,'Pontiac')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(60,'Porsche')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(61,'Renault')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(62,'Rolls-royce')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(63,'Rover')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(64,'Saab')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(65,'Santana')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(66,'Seat')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(67,'Skoda')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(68,'Smart')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(69,'Ssangyong')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(70,'Subaru')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(71,'Suzuki')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(72,'Talbot')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(73,'Tata')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(74,'Toyota')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(75,'Umm')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(76,'Vaz')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(77,'Volkswagen')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(78,'Volvo')
INSERT INTO Config.CarMake(CarMakeId,Name)Values(79,'Wartburg')

SET IDENTITY_INSERT Config.CarMake OFF

SET IDENTITY_INSERT Config.CarModel ON

INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(1,1,'500')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(1,2,'Grande Punto')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(1,3,'Punto Evo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(1,4,'500c')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(1,5,'695')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(1,6,'Punto')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(2,7,'155')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(2,8,'156')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(2,9,'159')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(2,10,'164')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(2,11,'145')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(2,12,'147')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(2,13,'146')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(2,14,'Gtv')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(2,15,'Spider')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(2,16,'166')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(2,17,'Gt')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(2,18,'Crosswagon')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(2,19,'Brera')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(2,20,'90')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(2,21,'75')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(2,22,'33')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(2,23,'Giulietta')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(2,24,'Sprint')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(2,25,'Mito')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(3,26,'Expander')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(3,27,'10')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(3,28,'24')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(3,29,'Dacia')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(4,30,'Rocsta')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(5,31,'Rocsta')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(6,32,'Db7')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(6,33,'V8')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(6,34,'Db9')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(6,35,'Vanquish')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(6,36,'V8 Vantage')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(6,37,'Vantage')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(6,38,'Dbs')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(6,39,'Volante')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(6,40,'Virage')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(6,41,'Vantage V8')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(6,42,'Vantage V12')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(6,43,'Rapide')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(6,44,'Cygnet')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,45,'80')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,46,'A4')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,47,'A6')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,48,'S6')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,49,'Coupe')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,50,'S2')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,51,'Rs2')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,52,'A8')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,53,'Cabriolet')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,54,'S8')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,55,'A3')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,56,'S4')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,57,'Tt')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,58,'S3')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,59,'Allroad Quattro')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,60,'Rs4')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,61,'A2')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,62,'Rs6')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,63,'Q7')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,64,'R8')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,65,'A5')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,66,'S5')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,67,'V8')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,68,'200')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,69,'100')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,70,'90')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,71,'Tts')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,72,'Q5')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,73,'A4 Allroad Quattro')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,74,'Tt Rs')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,75,'Rs5')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,76,'A1')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,77,'A7')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,78,'Rs3')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,79,'Q3')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,80,'A6 Allroad Quattro')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,81,'S7')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(7,82,'Sq5')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(8,83,'Mini')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(8,84,'Montego')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(8,85,'Maestro')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(8,86,'Metro')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(8,87,'Mini Moke')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(9,88,'Diesel')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(10,89,'Brooklands')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(10,90,'Turbo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(10,91,'Continental')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(10,92,'Azure')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(10,93,'Arnage')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(10,94,'Continental Gt')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(10,95,'Continental Flying Spur')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(10,96,'Turbo R')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(10,97,'Mulsanne')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(10,98,'Eight')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(10,99,'Continental Gtc')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(10,100,'Continental Supersports')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(11,101,'Freeclimber Diesel')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(12,102,'Serie 3')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(12,103,'Serie 5')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(12,104,'Compact')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(12,105,'Serie 7')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(12,106,'Serie 8')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(12,107,'Z3')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(12,108,'Z4')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(12,109,'Z8')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(12,110,'X5')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(12,111,'Serie 6')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(12,112,'X3')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(12,113,'Serie 1')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(12,114,'Z1')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(12,115,'X6')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(12,116,'X1')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(13,117,'Seville')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(13,118,'Sts')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(13,119,'El Dorado')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(13,120,'Cts')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(13,121,'Xlr')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(13,122,'Srx')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(13,123,'Escalade')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(13,124,'Bls')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(14,125,'Corvette')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(14,126,'Blazer')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(14,127,'Astro')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(14,128,'Nubira')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(14,129,'Evanda')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(14,130,'Trans Sport')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(14,131,'Camaro')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(14,132,'Matiz')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(14,133,'Alero')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(14,134,'Tahoe')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(14,135,'Tacuma')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(14,136,'Trailblazer')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(14,137,'Kalos')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(14,138,'Aveo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(14,139,'Lacetti')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(14,140,'Epica')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(14,141,'Captiva')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(14,142,'Hhr')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(14,143,'Cruze')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(14,144,'Spark')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(14,145,'Orlando')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(14,146,'Volt')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(14,147,'Malibu')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(15,148,'Vision')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(15,149,'300m')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(15,150,'Grand Voyager')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(15,151,'Viper')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(15,152,'Neon')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(15,153,'Voyager')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(15,154,'Stratus')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(15,155,'Sebring')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(15,156,'Sebring 200c')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(15,157,'New Yorker')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(15,158,'Pt Cruiser')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(15,159,'Crossfire')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(15,160,'300c')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(15,161,'Le Baron')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(15,162,'Saratoga')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,163,'Xantia')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,164,'Xm')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,165,'Ax')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,166,'Zx')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,167,'Evasion')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,168,'C8')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,169,'Saxo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,170,'C2')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,171,'Xsara')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,172,'C4')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,173,'Xsara Picasso')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,174,'C5')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,175,'C3')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,176,'C3 Pluriel')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,177,'C1')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,178,'C6')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,179,'Grand C4 Picasso')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,180,'C4 Picasso')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,181,'Ccrosser')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,182,'C15')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,183,'Jumper')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,184,'Jumpy')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,185,'Berlingo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,186,'Bx')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,187,'C25')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,188,'Cx')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,189,'Gsa')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,190,'Visa')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,191,'Lna')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,192,'2cv')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,193,'Nemo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,194,'C4 Sedan')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,195,'Berlingo First')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,196,'C3 Picasso')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,197,'Ds3')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,198,'Czero')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,199,'Ds4')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,200,'Ds5')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,201,'C4 Aircross')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(16,202,'Celysee')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(17,203,'Corvette')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(18,204,'Contac')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(18,205,'Logan')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(18,206,'Sandero')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(18,207,'Duster')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(18,208,'Lodgy')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(19,209,'Nexia')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(19,210,'Aranos')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(19,211,'Lanos')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(19,212,'Nubira')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(19,213,'Compact')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(19,214,'Nubira Compact')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(19,215,'Leganza')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(19,216,'Evanda')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(19,217,'Matiz')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(19,218,'Tacuma')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(19,219,'Kalos')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(19,220,'Lacetti')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(21,221,'Applause')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(21,222,'Charade')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(21,223,'Rocky')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(21,224,'Feroza')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(21,225,'Terios')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(21,226,'Sirion')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(22,227,'Serie Xj')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(22,228,'Xj')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(22,229,'Double Six')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(22,230,'Six')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(22,231,'Series Iii')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(23,232,'Viper')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(23,233,'Caliber')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(23,234,'Nitro')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(23,235,'Avenger')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(23,236,'Journey')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(24,237,'F355')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(24,238,'360')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(24,239,'F430')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(24,240,'F512 M')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(24,241,'550 Maranello')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(24,242,'575m Maranello')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(24,243,'599')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(24,244,'456')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(24,245,'456m')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(24,246,'612')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(24,247,'F50')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(24,248,'Enzo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(24,249,'Superamerica')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(24,250,'430')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(24,251,'348')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(24,252,'Testarossa')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(24,253,'512')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(24,254,'355')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(24,255,'F40')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(24,256,'412')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(24,257,'Mondial')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(24,258,'328')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(24,259,'California')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(24,260,'458')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(24,261,'Ff')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,262,'Croma')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,263,'Cinquecento')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,264,'Seicento')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,265,'Punto')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,266,'Grande Punto')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,267,'Panda')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,268,'Tipo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,269,'Coupe')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,270,'Uno')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,271,'Ulysse')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,272,'Tempra')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,273,'Marea')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,274,'Barchetta')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,275,'Bravo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,276,'Stilo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,277,'Brava')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,278,'Palio Weekend')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,279,'600')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,280,'Multipla')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,281,'Idea')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,282,'Sedici')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,283,'Linea')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,284,'500')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,285,'Fiorino')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,286,'Ducato')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,287,'Doblo Cargo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,288,'Doblo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,289,'Strada')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,290,'Regata')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,291,'Talento')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,292,'Argenta')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,293,'Ritmo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,294,'Punto Classic')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,295,'Qubo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,296,'Punto Evo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,297,'500c')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,298,'Freemont')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,299,'Panda Classic')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(25,300,'500l')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(26,301,'Maverick')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(26,302,'Escort')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(26,303,'Focus')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(26,304,'Mondeo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(26,305,'Scorpio')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(26,306,'Fiesta')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(26,307,'Probe')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(26,308,'Explorer')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(26,309,'Galaxy')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(26,310,'Ka')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(26,311,'Puma')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(26,312,'Cougar')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(26,313,'Focus Cmax')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(26,314,'Fusion')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(26,315,'Streetka')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(26,316,'Cmax')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(26,317,'Smax')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(26,318,'Transit')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(26,319,'Courier')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(26,320,'Ranger')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(26,321,'Sierra')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(26,322,'Orion')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(26,323,'Pick Up')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(26,324,'Capri')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(26,325,'Granada')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(26,326,'Kuga')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(26,327,'Grand Cmax')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(26,328,'Bmax')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(26,329,'Tourneo Custom')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(27,330,'Exceed')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(27,331,'Santamo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(27,332,'Super Exceed')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(29,333,'Accord')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(29,334,'Civic')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(29,335,'Crx')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(29,336,'Prelude')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(29,337,'Nsx')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(29,338,'Legend')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(29,339,'Crv')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(29,340,'Hrv')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(29,341,'Logo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(29,342,'S2000')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(29,343,'Stream')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(29,344,'Jazz')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(29,345,'Frv')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(29,346,'Concerto')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(29,347,'Insight')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(29,348,'Crz')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(30,349,'H2')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(30,350,'H3')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(30,351,'H3t')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(31,352,'Lantra')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(31,353,'Sonata')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(31,354,'Elantra')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(31,355,'Accent')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(31,356,'Scoupe')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(31,357,'Coupe')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(31,358,'Atos')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(31,359,'H1')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(31,360,'Atos Prime')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(31,361,'Xg')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(31,362,'Trajet')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(31,363,'Santa Fe')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(31,364,'Terracan')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(31,365,'Matrix')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(31,366,'Getz')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(31,367,'Tucson')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(31,368,'I30')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(31,369,'Pony')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(31,370,'Grandeur')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(31,371,'I10')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(31,372,'I800')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(31,373,'Sonata Fl')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(31,374,'Ix55')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(31,375,'I20')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(31,376,'Ix35')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(31,377,'Ix20')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(31,378,'Genesis')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(31,379,'I40')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(31,380,'Veloster')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(32,381,'G')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(32,382,'Ex')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(32,383,'Fx')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(32,384,'M')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(33,385,'Elba')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(33,386,'Minitre')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(34,387,'Trooper')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(34,388,'Pick Up')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(34,389,'D Max')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(34,390,'Rodeo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(34,391,'Dmax')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(34,392,'Trroper')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(35,393,'Daily')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(35,394,'Massif')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(36,395,'Daily')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(36,396,'Duty')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(37,397,'Serie Xj')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(37,398,'Serie Xk')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(37,399,'Xj')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(37,400,'Stype')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(37,401,'Xf')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(37,402,'Xtype')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(38,403,'Wrangler')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(38,404,'Cherokee')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(38,405,'Grand Cherokee')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(38,406,'Commander')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(38,407,'Compass')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(38,408,'Wrangler Unlimited')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(38,409,'Patriot')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(39,410,'Sportage')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(39,411,'Sephia')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(39,412,'Sephia Ii')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(39,413,'Pride')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(39,414,'Clarus')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(39,415,'Shuma')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(39,416,'Carnival')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(39,417,'Joice')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(39,418,'Magentis')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(39,419,'Carens')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(39,420,'Rio')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(39,421,'Cerato')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(39,422,'Sorento')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(39,423,'Opirus')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(39,424,'Picanto')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(39,425,'Ceed')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(39,426,'Ceed Sporty Wagon')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(39,427,'Proceed')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(39,428,'K2500 Frontier')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(39,429,'K2500')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(39,430,'Soul')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(39,431,'Venga')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(39,432,'Optima')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(39,433,'Ceed Sportswagon')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(40,434,'Samara')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(40,435,'Niva')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(40,436,'Sagona')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(40,437,'Stawra 2110')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(40,438,'214')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(40,439,'Kalina')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(40,440,'Serie 2100')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(40,441,'Priora')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(41,442,'Gallardo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(41,443,'Murcielago')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(41,444,'Aventador')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(42,445,'Delta')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(42,446,'K')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(42,447,'Y10')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(42,448,'Dedra')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(42,449,'Lybra')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(42,450,'Z')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(42,451,'Y')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(42,452,'Ypsilon')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(42,453,'Thesis')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(42,454,'Phedra')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(42,455,'Musa')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(42,456,'Thema')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(42,457,'Zeta')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(42,458,'Kappa')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(42,459,'Trevi')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(42,460,'Prisma')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(42,461,'A112')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(42,462,'Ypsilon Elefantino')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(42,463,'Voyager')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(43,464,'Range Rover')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(43,465,'Defender')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(43,466,'Discovery')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(43,467,'Freelander')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(43,468,'Range Rover Sport')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(43,469,'Discovery 4')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(43,470,'Range Rover Evoque')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(44,471,'Maxus')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(45,472,'Ls400')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(45,473,'Ls430')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(45,474,'Gs300')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(45,475,'Is200')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(45,476,'Rx300')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(45,477,'Gs430')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(45,478,'Gs460')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(45,479,'Sc430')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(45,480,'Is300')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(45,481,'Is250')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(45,482,'Rx400h')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(45,483,'Is220d')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(45,484,'Rx350')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(45,485,'Gs450h')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(45,486,'Ls460')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(45,487,'Ls600h')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(45,488,'Ls')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(45,489,'Gs')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(45,490,'Is')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(45,491,'Sc')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(45,492,'Rx')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(45,493,'Ct')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(46,494,'Elise')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(46,495,'Exige')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(47,496,'Bolero Pickup')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(47,497,'Goa Pickup')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(47,498,'Goa')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(47,499,'Cj')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(47,500,'Pikup')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(47,501,'Thar')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(48,502,'Ghibli')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(48,503,'Shamal')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(48,504,'Quattroporte')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(48,505,'3200 Gt')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(48,506,'Coupe')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(48,507,'Spyder')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(48,508,'Gransport')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(48,509,'Granturismo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(48,510,'430')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(48,511,'Biturbo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(48,512,'228')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(48,513,'224')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(48,514,'Grancabrio')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(49,515,'Maybach')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(50,516,'Xedos 6')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(50,517,'626')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(50,518,'121')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(50,519,'Xedos 9')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(50,520,'323')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(50,521,'Mx3')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(50,522,'Rx7')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(50,523,'Mx5')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(50,524,'Mazda3')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(50,525,'Mpv')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(50,526,'Demio')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(50,527,'Premacy')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(50,528,'Tribute')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(50,529,'Mazda6')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(50,530,'Mazda2')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(50,531,'Rx8')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(50,532,'Mazda5')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(50,533,'Cx7')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(50,534,'Serie B')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(50,535,'B2500')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(50,536,'Bt50')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(50,537,'Mx6')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(50,538,'929')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(50,539,'Cx5')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,540,'Clase C')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,541,'Clase E')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,542,'Clase Sl')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,543,'Clase S')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,544,'Clase Cl')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,545,'Clase G')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,546,'Clase Slk')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,547,'Clase V')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,548,'Viano')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,549,'Clase Clk')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,550,'Clase A')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,551,'Clase M')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,552,'Vaneo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,553,'Slklasse')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,554,'Slr Mclaren')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,555,'Clase Cls')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,556,'Clase R')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,557,'Clase Gl')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,558,'Clase B')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,559,'100d')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,560,'140d')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,561,'180d')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,562,'Sprinter')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,563,'Vito')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,564,'Transporter')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,565,'280')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,566,'220')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,567,'200')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,568,'190')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,569,'600')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,570,'400')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,571,'Clase Sl R129')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,572,'300')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,573,'500')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,574,'420')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,575,'260')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,576,'230')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,577,'Clase Clc')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,578,'Clase Glk')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(51,579,'Sls Amg')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(52,580,'Mgf')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(52,581,'Tf')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(52,582,'Zr')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(52,583,'Zs')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(52,584,'Zt')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(52,585,'Ztt')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(52,586,'Mini')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(52,587,'Countryman')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(52,588,'Paceman')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(54,589,'Montero')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(54,590,'Galant')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(54,591,'Colt')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(54,592,'Space Wagon')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(54,593,'Space Runner')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(54,594,'Space Gear')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(54,595,'3000 Gt')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(54,596,'Carisma')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(54,597,'Eclipse')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(54,598,'Space Star')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(54,599,'Montero Sport')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(54,600,'Montero Io')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(54,601,'Outlander')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(54,602,'Lancer')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(54,603,'Grandis')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(54,604,'L200')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(54,605,'Canter')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(54,606,'300 Gt')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(54,607,'Asx')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(54,608,'Imiev')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(55,609,'44')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(55,610,'Plus 8')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(55,611,'Aero 8')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(55,612,'V6')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(55,613,'Roadster')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(55,614,'4')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(55,615,'Plus 4')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,616,'Terrano Ii')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,617,'Terrano')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,618,'Micra')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,619,'Sunny')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,620,'Primera')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,621,'Serena')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,622,'Patrol')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,623,'Maxima Qx')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,624,'200 Sx')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,625,'300 Zx')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,626,'Patrol Gr')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,627,'100 Nx')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,628,'Almera')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,629,'Pathfinder')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,630,'Almera Tino')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,631,'Xtrail')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,632,'350z')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,633,'Murano')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,634,'Note')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,635,'Qashqai')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,636,'Tiida')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,637,'Vanette')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,638,'Trade')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,639,'Vanette Cargo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,640,'Pickup')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,641,'Navara')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,642,'Cabstar E')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,643,'Cabstar')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,644,'Maxima')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,645,'Camion')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,646,'Prairie')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,647,'Bluebird')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,648,'Np300 Pick Up')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,649,'Qashqai2')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,650,'Pixo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,651,'Gtr')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,652,'370z')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,653,'Cube')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,654,'Juke')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,655,'Leaf')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(56,656,'Evalia')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(57,657,'Astra')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(57,658,'Vectra')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(57,659,'Calibra')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(57,660,'Corsa')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(57,661,'Omega')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(57,662,'Frontera')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(57,663,'Tigra')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(57,664,'Monterey')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(57,665,'Sintra')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(57,666,'Zafira')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(57,667,'Agila')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(57,668,'Speedster')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(57,669,'Signum')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(57,670,'Meriva')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(57,671,'Antara')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(57,672,'Gt')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(57,673,'Combo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(57,674,'Movano')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(57,675,'Vivaro')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(57,676,'Kadett')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(57,677,'Monza')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(57,678,'Senator')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(57,679,'Rekord')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(57,680,'Manta')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(57,681,'Ascona')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(57,682,'Insignia')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(57,683,'Zafira Tourer')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(57,684,'Ampera')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(57,685,'Mokka')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(57,686,'Adam')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,687,'306')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,688,'605')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,689,'106')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,690,'205')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,691,'405')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,692,'406')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,693,'806')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,694,'807')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,695,'407')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,696,'307')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,697,'206')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,698,'607')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,699,'308')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,700,'307 Sw')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,701,'206 Sw')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,702,'407 Sw')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,703,'1007')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,704,'107')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,705,'207')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,706,'4007')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,707,'Boxer')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,708,'Partner')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,709,'J5')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,710,'604')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,711,'505')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,712,'309')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,713,'Bipper')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,714,'Partner Origin')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,715,'3008')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,716,'5008')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,717,'Rcz')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,718,'508')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,719,'Ion')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,720,'208')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(58,721,'4008')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(59,722,'Trans Sport')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(59,723,'Firebird')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(59,724,'Trans Am')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(60,725,'911')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(60,726,'Boxster')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(60,727,'Cayenne')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(60,728,'Carrera Gt')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(60,729,'Cayman')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(60,730,'928')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(60,731,'968')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(60,732,'944')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(60,733,'924')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(60,734,'Panamera')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(60,735,'918')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,736,'Megane')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,737,'Safrane')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,738,'Laguna')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,739,'Clio')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,740,'Twingo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,741,'Nevada')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,742,'Espace')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,743,'Spider')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,744,'Scenic')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,745,'Grand Espace')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,746,'Avantime')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,747,'Vel Satis')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,748,'Grand Scenic')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,749,'Clio Campus')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,750,'Modus')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,751,'Express')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,752,'Trafic')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,753,'Master')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,754,'Kangoo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,755,'Mascott')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,756,'Master Propulsion')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,757,'Maxity')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,758,'R19')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,759,'R25')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,760,'R5')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,761,'R21')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,762,'R4')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,763,'Alpine')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,764,'Fuego')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,765,'R18')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,766,'R11')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,767,'R9')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,768,'R6')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,769,'Grand Modus')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,770,'Kangoo Combi')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,771,'Koleos')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,772,'Fluence')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,773,'Wind')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,774,'Latitude')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(61,775,'Grand Kangoo Combi')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(62,776,'Siver Dawn')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(62,777,'Silver Spur')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(62,778,'Park Ward')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(62,779,'Silver Seraph')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(62,780,'Corniche')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(62,781,'Phantom')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(62,782,'Touring')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(62,783,'Silvier')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(63,784,'800')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(63,785,'600')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(63,786,'100')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(63,787,'200')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(63,788,'Coupe')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(63,789,'400')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(63,790,'45')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(63,791,'Cabriolet')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(63,792,'25')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(63,793,'Mini')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(63,794,'75')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(63,795,'Streetwise')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(63,796,'Sd')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(64,797,'900')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(64,798,'93')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(64,799,'9000')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(64,800,'95')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(64,801,'93x')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(64,802,'94x')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(65,803,'300')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(65,804,'350')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(65,805,'Anibal')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(65,806,'Anibal Pick Up')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(66,807,'Ibiza')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(66,808,'Cordoba')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(66,809,'Toledo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(66,810,'Marbella')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(66,811,'Alhambra')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(66,812,'Arosa')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(66,813,'Leon')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(66,814,'Altea')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(66,815,'Altea Xl')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(66,816,'Altea Freetrack')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(66,817,'Terra')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(66,818,'Inca')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(66,819,'Malaga')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(66,820,'Ronda')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(66,821,'Exeo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(66,822,'Mii')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(67,823,'Felicia')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(67,824,'Forman')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(67,825,'Octavia')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(67,826,'Octavia Tour')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(67,827,'Fabia')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(67,828,'Superb')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(67,829,'Roomster')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(67,830,'Scout')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(67,831,'Pickup')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(67,832,'Favorit')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(67,833,'130')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(67,834,'S')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(67,835,'Yeti')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(67,836,'Citigo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(67,837,'Rapid')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(68,838,'Smart')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(68,839,'Citycoupe')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(68,840,'Fortwo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(68,841,'Cabrio')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(68,842,'Crossblade')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(68,843,'Roadster')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(68,844,'Forfour')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(69,845,'Korando')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(69,846,'Family')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(69,847,'K4d')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(69,848,'Musso')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(69,849,'Korando Kj')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(69,850,'Rexton')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(69,851,'Rexton Ii')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(69,852,'Rodius')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(69,853,'Kyron')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(69,854,'Actyon')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(69,855,'Sports Pick Up')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(69,856,'Actyon Sports Pick Up')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(69,857,'Kodando')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(70,858,'Legacy')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(70,859,'Impreza')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(70,860,'Svx')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(70,861,'Justy')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(70,862,'Outback')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(70,863,'Forester')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(70,864,'G3x Justy')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(70,865,'B9 Tribeca')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(70,866,'Xt')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(70,867,'1800')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(70,868,'Tribeca')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(70,869,'Wrx Sti')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(70,870,'Trezia')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(70,871,'Xv')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(70,872,'Brz')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(71,873,'Maruti')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(71,874,'Swift')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(71,875,'Vitara')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(71,876,'Baleno')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(71,877,'Samurai')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(71,878,'Alto')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(71,879,'Wagon R')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(71,880,'Jimny')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(71,881,'Grand Vitara')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(71,882,'Ignis')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(71,883,'Liana')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(71,884,'Grand Vitara Xl7')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(71,885,'Sx4')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(71,886,'Splash')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(71,887,'Kizashi')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(72,888,'Samba')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(72,889,'Tagora')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(72,890,'Solara')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(72,891,'Horizon')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(73,892,'Telcosport')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(73,893,'Telco')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(73,894,'Sumo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(73,895,'Safari')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(73,896,'Indica')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(73,897,'Indigo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(73,898,'Grand Safari')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(73,899,'Tl Pick Up')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(73,900,'Xenon Pick Up')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(73,901,'Vista')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(73,902,'Xenon')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(73,903,'Aria')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(74,904,'Carina E')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(74,905,'4runner')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(74,906,'Camry')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(74,907,'Rav4')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(74,908,'Celica')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(74,909,'Supra')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(74,910,'Paseo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(74,911,'Land Cruiser 80')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(74,912,'Land Cruiser 100')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(74,913,'Land Cruiser')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(74,914,'Land Cruiser 90')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(74,915,'Corolla')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(74,916,'Auris')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(74,917,'Avensis')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(74,918,'Picnic')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(74,919,'Yaris')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(74,920,'Yaris Verso')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(74,921,'Mr2')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(74,922,'Previa')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(74,923,'Prius')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(74,924,'Avensis Verso')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(74,925,'Corolla Verso')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(74,926,'Corolla Sedan')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(74,927,'Aygo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(74,928,'Hilux')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(74,929,'Dyna')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(74,930,'Land Cruiser 200')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(74,931,'Verso')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(74,932,'Iq')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(74,933,'Urban Cruiser')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(74,934,'Gt86')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(75,935,'100')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(75,936,'121')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(76,937,'214')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(76,938,'110 Stawra')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(76,939,'111 Stawra')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(76,940,'215')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(76,941,'112 Stawra')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(77,942,'Passat')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(77,943,'Golf')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(77,944,'Vento')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(77,945,'Polo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(77,946,'Corrado')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(77,947,'Sharan')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(77,948,'Lupo')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(77,949,'Bora')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(77,950,'Jetta')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(77,951,'New Beetle')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(77,952,'Phaeton')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(77,953,'Touareg')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(77,954,'Touran')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(77,955,'Multivan')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(77,956,'Caddy')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(77,957,'Golf Plus')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(77,958,'Fox')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(77,959,'Eos')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(77,960,'Caravelle')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(77,961,'Tiguan')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(77,962,'Transporter')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(77,963,'Lt')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(77,964,'Taro')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(77,965,'Crafter')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(77,966,'California')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(77,967,'Santana')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(77,968,'Scirocco')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(77,969,'Passat Cc')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(77,970,'Amarok')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(77,971,'Beetle')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(77,972,'Up')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(77,973,'Cc')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(78,974,'440')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(78,975,'850')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(78,976,'S70')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(78,977,'V70')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(78,978,'V70 Classic')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(78,979,'940')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(78,980,'480')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(78,981,'460')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(78,982,'960')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(78,983,'S90')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(78,984,'V90')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(78,985,'Classic')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(78,986,'S40')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(78,987,'V40')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(78,988,'V50')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(78,989,'V70 Xc')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(78,990,'Xc70')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(78,991,'C70')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(78,992,'S80')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(78,993,'S60')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(78,994,'Xc90')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(78,995,'C30')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(78,996,'780')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(78,997,'760')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(78,998,'740')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(78,999,'240')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(78,1000,'360')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(78,1001,'340')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(78,1002,'Xc60')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(78,1003,'V60')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(78,1004,'V40 Cross Country')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(79,1005,'353')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(53,1006,'Mini')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(53,1007,'Countryman')
INSERT INTO Config.CarModel(CarMakeId,CarModelId,Name)VALUES(53,1008,'Paceman')
SET IDENTITY_INSERT Config.CarModel OFF


--=================CLIENT================================-
SET IDENTITY_INSERT Catalog.Client ON

INSERT INTO [Catalog].[Client]
           ([ClientId],[CityId],[Code],[Name],[BusinessName],[LegalRepresentative],[FTR],[TaxAddress],[Address],[ZipCode],[Entrance],[Email],[Phone],[IsActive],[UpdDate],UpdUser)
     VALUES
           (0,2309,'0000','CLIENTE POR DEFECTO','CLIENTE POR DEFECTO',null,'CAAA060615AA2','DIRECCION POR DEFECTO','DIRECCION POR DEFECTO','',GETDATE(),'','',1,GETDATE(),'Administrador')

SET IDENTITY_INSERT Catalog.Client OFF

--=======Variables==============================================

INSERT INTO [Config].[Variable]
           ([Name],[Value])
     VALUES
           ('StorePercentage','100')

INSERT INTO [Config].[Variable]
           ([Name],[Value])
     VALUES
           ('DealerPercentage','75')

INSERT INTO [Config].[Variable]
           ([Name],[Value])
     VALUES
           ('WholesalerPercentage','25')




--=============User Administrator================================== "Administrador01."======================================================================
INSERT INTO [Security].[AspNetUsers]
           ([Id],[Email],[EmailConfirmed],[PasswordHash],[SecurityStamp],[PhoneNumberConfirmed],[TwoFactorEnabled],[LockoutEnabled],[AccessFailedCount],[UserName],ComissionForSale)
     VALUES
           (N'384a8104-cbc6-40a5-a27c-2947263e0fcc','arkhameng@gmail.com',1,N'AK6X8N0K4cOnpKk8DOHYYocTWSNB/yLXybmoExtH1AE7+QsgZQ07NNg6NL5DXiKJqQ==',N'c7d38da5-c068-4838-97b1-b563eaf4a53b',0,0,1,0,'Administrador',0)



--====================BRANCHES===============================================================================================================================

INSERT INTO [Config].[Branch]([Name])
     VALUES('Zapopan')

INSERT INTO [Config].[Branch]([Name])
     VALUES('Cosamaloapan')

INSERT INTO [Config].[Branch]([Name])
     VALUES('Cuahutemoc')

INSERT INTO [Config].[Branch]([Name])
     VALUES('Miguel Aleman')

--====================USER BRANCHES==========================================================

INSERT INTO [Config].[UserBranch]([BranchId],[UserId])
     VALUES(1,N'384a8104-cbc6-40a5-a27c-2947263e0fcc')

INSERT INTO [Config].[UserBranch]([BranchId],[UserId])
     VALUES(2,N'384a8104-cbc6-40a5-a27c-2947263e0fcc')

INSERT INTO [Config].[UserBranch]([BranchId],[UserId])
     VALUES(3,N'384a8104-cbc6-40a5-a27c-2947263e0fcc')

INSERT INTO [Config].[UserBranch]([BranchId],[UserId])
     VALUES(4,N'384a8104-cbc6-40a5-a27c-2947263e0fcc')

--=================PROVIDER DEFAULT==================================================
--SET IDENTITY_INSERT [Catalog].[Provider] ON

--INSERT INTO [Catalog].[Provider]
--           (ProviderId,[Code],[Name],[Address],[CityId],[Phone],[IsActive],[UpdDate])
--     VALUES
--           (0,'0000','PROVEEDOR POR DEFECTO','DIRECCION POR DEFECTO',2309,'9999999999',1,GETDATE())


--SET IDENTITY_INSERT [Catalog].[Provider] OFF

--==============ROLES======================================


INSERT [Security].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'6a25bb08-f81d-4d7e-8b4b-b1aaa2c73b19', N'Vendedor', NULL, N'IdentityRole')
GO
INSERT [Security].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'8c30e201-5f65-42ab-95a3-9a673c543de7', N'Almacenista', NULL, N'IdentityRole')
GO
INSERT [Security].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'a2bd31ac-7150-41f2-b875-3a01eb2ff518', N'Cajero', NULL, N'IdentityRole')
GO
INSERT [Security].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'b0468515-731b-4cce-98ef-caf3e4a86cd9', N'Administrador', NULL, N'IdentityRole')
GO
INSERT [Security].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'b7ca23c8-6f3d-4ecb-976a-b8ef33563d9b', N'Supervisor', NULL, N'IdentityRole')
GO
INSERT [Security].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'e0219f0a-25e2-4c93-a718-b143a0450c0a', N'Capturista', NULL, N'IdentityRole')

--======================USER ROLES================================
GO
INSERT INTO [Security].[AspNetUserRoles] ([UserId],[RoleId])
     VALUES(N'384a8104-cbc6-40a5-a27c-2947263e0fcc' ,N'6a25bb08-f81d-4d7e-8b4b-b1aaa2c73b19')

GO
INSERT INTO [Security].[AspNetUserRoles] ([UserId],[RoleId])
     VALUES(N'384a8104-cbc6-40a5-a27c-2947263e0fcc' ,N'8c30e201-5f65-42ab-95a3-9a673c543de7')

GO

INSERT INTO [Security].[AspNetUserRoles] ([UserId],[RoleId])
     VALUES(N'384a8104-cbc6-40a5-a27c-2947263e0fcc' ,N'a2bd31ac-7150-41f2-b875-3a01eb2ff518')

GO

INSERT INTO [Security].[AspNetUserRoles] ([UserId],[RoleId])
     VALUES(N'384a8104-cbc6-40a5-a27c-2947263e0fcc' ,N'b0468515-731b-4cce-98ef-caf3e4a86cd9')

GO
INSERT INTO [Security].[AspNetUserRoles] ([UserId],[RoleId])
     VALUES(N'384a8104-cbc6-40a5-a27c-2947263e0fcc' ,N'b7ca23c8-6f3d-4ecb-976a-b8ef33563d9b')

GO
INSERT INTO [Security].[AspNetUserRoles] ([UserId],[RoleId])
     VALUES(N'384a8104-cbc6-40a5-a27c-2947263e0fcc' ,N'e0219f0a-25e2-4c93-a718-b143a0450c0a')



--STORED PROCEDURES
GO
CREATE PROCEDURE [Catalog].[DelExternalProduct]
(
   @ProviderId INT
)
AS
BEGIN
	DECLARE @Count INT = 0

	SELECT @Count = COUNT(*)
	FROM [Catalog].[ExternalProduct]
	WHERE ProviderId = @ProviderId

	IF @Count > 0
	BEGIN
		DELETE FROM [Catalog].[ExternalProduct]
		WHERE ProviderId = @ProviderId
	END

END


--=======CATEGORIAS======================--
GO
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Catalizadores','12161600',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Acabados y revestimientos exterior para vehículos','25172600',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Accesorios y componentes de motor','26101700',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Aisladores y amortiguadores de vibración de vehículos','25174900',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Baterías, pilas y accesorios','26111700',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Cable automotriz','26121800',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Chasis automotriz','25181600',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Componentes de ciclo no motorizado o accesorios','25174700',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Componentes de la transmisión','26111800',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Componentes de motores o generadores','26101400',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Componentes de sistema de suspensión','25172000',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Componentes eléctricos','25173900',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Componentes y sistemas de seguridad de vehículo','25172100',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Componentes y sistemas de vehículos','25174800',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Controles de emisión y de escape','25173700',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Cubiertas automotrices','31181800',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Depósitos y sistemas de combustible','25172400',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Embalajes y empaquetaduras','31181700',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Embragues','26111900',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Equipo de calefacción y piezas y accesorios','40101800',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Equipo de medición y prueba de vehículos','25191800',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Equipo para el mantenimiento de vehículo','25191700',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Filtros','40161500',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Generadores de potencia','26111600',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Iluminación exterior para vehículos','25172900',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Iluminación interior para vehículos','25173000',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Limpiaparabrisas','25171500',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Motores','26101500',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Motores eléctricos de corriente alterna AC','26101100',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Motores eléctricos de corriente directa DC','26101200',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Motores no eléctricos','26101300',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Neumáticos y cámaras de neumáticos','25172500',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Piezas y accesorios de embragues','26112000',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Preparados lubricantes','15121500',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Puertas para vehículos','25172200',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Remolques para productos y materiales','25181700',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Ruedas y acabados','25171900',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Sistema de refrigerar de motor','25174000',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Sistemas de asientos del vehículo','25174600',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Sistemas de cargado eléctrico de vehículos','25175000',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Sistemas de control medioambiental','25172700',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Sistemas de control principal','25173300',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Sistemas de descarchado y antiniebla','25171600',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Sistemas de frenado y componentes','25171700',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Sistemas de tren de transmisión','25173800',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Sistemas del interior de vehículos','25174400',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Sistemas y componentes hidráulicos','25172800',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Sistemas y equipo de apoyo para transporte aéreo','25191500',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Sistemas y equipo de apoyo para transporte espacial','25191600',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Transmisión de energía cinética','26111500',0)
INSERT INTO [Config].[Category] (Name, SatCode, Commission) VALUES('Ventanas y parabrisas para vehículos','25172300',0)
GO


--===INITIAL LOAD PRODUCT==============
ALTER PROCEDURE [Catalog].[LoadProduct]
(
     @Code nvarchar(30)
    ,@Name nvarchar(200)
    ,@MinQuantity float
    ,@BuyPrice float
    ,@StorePercentage int
    ,@DealerPercentage int
    ,@WholesalerPercentage int
    ,@StorePrice float
    ,@WholesalerPrice float
    ,@DealerPrice float
    ,@TradeMark nvarchar(50)
    ,@Unit nvarchar(20)
    ,@UpdUser nvarchar(100)
)
AS
BEGIN

DECLARE @UpdDate DATETIME = GETDATE()

INSERT INTO [Catalog].[Product]
           ([CategoryId]
           ,[Code]
           ,[Name]
           ,[MinQuantity]           
           ,[BuyPrice]
           ,[StorePercentage]
           ,[DealerPercentage]
           ,[WholesalerPercentage]
           ,[StorePrice]
           ,[WholesalerPrice]
           ,[DealerPrice]
           ,[PackagePrice]
           ,[ProductType]
           ,[TradeMark]
           ,[Unit]
           ,[IsActive]
           ,[UpdDate]
           ,[UpdUser])
     VALUES
           (1
           ,@Code
           ,@Name
           ,@MinQuantity
           ,@BuyPrice
           ,@StorePercentage
           ,@DealerPercentage
           ,@WholesalerPercentage
           ,@StorePrice
           ,@WholesalerPrice
           ,@DealerPrice
           ,0
           ,0
           ,@TradeMark 
           ,@Unit 
           ,1 
           ,@UpdDate
           ,@UpdUser)
END
