create database db_ecommerce;
use db_ecommerce;

create table tblCliente(
	idUsu int auto_increment,
    cpf decimal(11,0) unique,
    primary key(idUsu, cpf),
    cep decimal(8,0), /*FK*/
    numResidencia int not null,
    complemento varchar(100),
    email varchar(150), /*FK*/
    nomeUsu varchar(150) not null,
    nomeCompleto varchar(200) not null,
    avaliacaoMedica varchar(100) not null,
    dataNasc date not null,
    tel varchar(11) not null,
    foto varchar(200) null,
    senha varchar(150) not null,
    nivelAcesso boolean not null
);


-- select * from tblCliente;


create table tblProduto(
	codProduto int primary key auto_increment,
    nomeProd varchar(150) not null,
    dataFab datetime not null,
    dataValidade date not null,
    precoUnitario decimal(7,2) not null,
    qtdEstoque int not null,
    descCurta varchar(200) not null,
    descDetalhada text not null,
    peso decimal(5,2) not null,
    fotoProd varchar(200),
    no_carrinho bool,
    destaques bool,
    codCategoria int /*FK*/
);

create table tblCategoria(
	codCategoria int primary key auto_increment,
    nomeCategoria varchar(200)
);

create table tblCarrinhoCompras(
	idCarrinho int primary key auto_increment,
    idUsu int, /*FK*/
	valorTotal decimal(7, 2) not null,
    frete decimal(5, 2) not null
);
select * from tblCarrinhoCompras;
delete from tblCarrinhoCompras;


create table tblPagamento(
	idPagamento int primary key auto_increment,
    idUsu int, /*FK*/
    idCarrinho int not null, /*FK*/
    formaPag varchar(50) not null,
    valorTotal decimal(7, 2) not null,
    dataHoraPag datetime not null default current_timestamp,
    statusPag varchar(50) not null
);
select * from tblPagamento;

create table tblAvaliacao(
	idAvaliacao int primary key auto_increment,
    codProduto int not null, /*FK*/
    idUsu int, /*FK*/
    qtdEstrela int not null,
    comentario text not null,
);

/*
insert into tblAvaliacao values (default, 2, 1, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum feugiat sem nisl, non gravida erat posuere eget. Etiam condimentum, urna vitae cursus ullamcorper, urna nunc;", 38)
insert into tblAvaliacao values (default, 1, 4, "Teste de avaliacao", 38)


select * from tblAvaliacao a inner join tblCliente u on a.idUsu = u.idUsu where a.codProduto = @codProduto
select * from tblCliente
*/

create table tblEndereco(
	cep decimal(8,0) primary key,
    Logradouro varchar(200) not null,
    idBairro int not null /*FK*/
);

create table tblBairro(
	idBairro int primary key auto_increment,
    bairro varchar(200) not null
);

create table tblCartao(
	codCartao decimal(16,0) primary key,
    idUsu int, /*FK*/
    nomeTitular varchar(150) not null,
    tipoCartao bit not null,
    CVV decimal(3,0) not null,
    dataValidade date not null
);


alter table tblCliente add constraint fk_cliente_endereco_cep foreign key (cep) references tblEndereco(cep);

alter table tblProduto add constraint fk_produto_categoria_codcategoria foreign key (codCategoria) references tblCategoria(codCategoria);

alter table tblCarrinhoCompras add constraint fk_carrinho_cliente_idusu foreign key (idUsu) references tblCliente(idUsu);
alter table tblCarrinhoCompras add constraint fk_carrinho_produto_codproduto foreign key (codProduto) references tblProduto(codProduto);

alter table tblPagamento add constraint fk_pagamento_cliente_idusu foreign key (idUsu) references tblCliente(idUsu);
alter table tblPagamento add constraint fk_pagamento_carrinho_idcarrinho foreign key (idCarrinho) references tblCarrinhoCompras(idCarrinho);

alter table tblAvaliacao add constraint fk_avaliacao_cliente_idusu foreign key (idUsu) references tblCliente(idUsu);

alter table tblEndereco add constraint fk_endereco_bairro_idbairro foreign key (idBairro) references tblBairro(idBairro);

alter table tblCartao add constraint fk_cartao_usuario_idusu foreign key (idUsu) references tblCliente(idUsu);


ALTER TABLE tblCartao DROP PRIMARY KEY;
ALTER TABLE tblCartao ADD PRIMARY KEY (codCartao, idUsu);




-- drop database db_ecommerce;

/*------------------------------------------------------------------------------------------PROCEDURES---------------------------------------------------------------------------------*/

/*------------------------------------------------------------------------------------------BAIRRO---------------------------------------------------------------------------------*/

delimiter &&
CREATE PROCEDURE ipBairro(vBairro varchar(200))
begin
	IF NOT EXISTS (SELECT bairro from tblBairro where bairro = vBairro) THEN
		INSERT INTO tblBairro(bairro) values (vBairro);
	END IF;
END;
&&


/*------------------------------------------------------------------------------------------ENDEREÇO---------------------------------------------------------------------------------*/

delimiter &&
CREATE PROCEDURE ipEndereco(vCep decimal(8,0), vLogradouro varchar(200), vBairro varchar(200))
begin
IF NOT EXISTS(SELECT cep from tblEndereco where cep = vCep) THEN
	IF NOT EXISTS (SELECT bairro from tblBairro where bairro = vBairro) THEN
		INSERT INTO tblBairro (bairro) values (vBairro);
	END IF;
INSERT INTO tblEndereco (cep, Logradouro, idBairro) values (vCep, vLogradouro, (SELECT idBairro from tblBairro where bairro = vBairro));
END IF;
END;
&&



/*------------------------------------------------------------------------------------------LOGIN E CLIENTE---------------------------------------------------------------------------------*/

/*
Dicionario de variaveis
    
(tblCliente)    
    vCartao : codCartao 
    vNUsu : nomeUsu 
    vNCusu : nomeCompleto
    vMedica : avaliacaoMedica
    vNasc : dataNasc
*/

delimiter &&
CREATE PROCEDURE 
ipCliente(vEmail varchar(150), vSenha varchar(150), vCpf decimal(11,0), vCep decimal(8,0), vLogradouro varchar(200), vBairro varchar(200), vNumCasa int, vComplemento varchar(100), vNUsu varchar(150), vNCUsu varchar(200), vMedica varchar(100), vNasc date, vTel varchar(11), vFoto varchar(200))
BEGIN
	IF NOT EXISTS(SELECT cep from tblEndereco where cep = vCep) THEN
	
		IF NOT EXISTS (SELECT bairro from tblBairro where bairro = vBairro) THEN
		
			INSERT INTO tblBairro (bairro) values (vBairro);
		
		END IF;

		INSERT INTO tblEndereco (cep, Logradouro, idBairro) values (vCep, vLogradouro, (SELECT idBairro from tblBairro where bairro = vBairro));

	END IF;

    IF NOT EXISTS(SELECT idUsu from tblCliente where cpf = vCpf) THEN	
		INSERT INTO tblCliente (idUsu, cpf, cep, numResidencia, complemento, email, nomeUsu, nomeCompleto, avaliacaoMedica, dataNasc, tel, foto, senha, nivelAcesso) values (default, vCpf, vCep, vNumCasa, vComplemento, vEmail, vNUsu, vNCUsu, vMedica, vNasc, vTel, vFoto, vSenha, 1);
	END IF;

END;
&&



/*
call ipCliente("matheus@gmail.com", "12345678", 12345678910, 02805000, "Rua Manuel José de Almeida", null, 260, "Sobrado", "MGRSongs", "Matheus Gama Russi", "Debilitado", "2006-07-06", "942747859", "path")

select * from tblCliente;
select * from tblEndereco;
select * from tblBairro;

delete from tblCliente;
delete from tblEndereco;
delete from tblBairro;

set sql_safe_updates = 0
*/

/*------------------------------------------------------------------------------------------PRODUTO---------------------------------------------------------------------------------*/

/*
Dicionario de variaveis

(tblProduto)
	vDFab : dataFab
    vDVal : dataValidade
    vPUni : precoUnitario
    vQtd : QtdEstoque
    vDCurta : descCurta
    vDDet : descDetalhada
    vFProd : fotoProd
    vCarrinho : no_carrinho
*/

delimiter $$
CREATE PROCEDURE
ipProduto(vNomeProd varchar(150), vDFab datetime, vDVal date, vPUni decimal(7,2), vQtd int, vDCurta varchar(200), vDDet text, vPeso decimal(5,2), vFProd varchar(200), vCarrinho bool, vDestaques bool, vCodCat int)
BEGIN
	IF NOT EXISTS (SELECT codProduto from tblProduto where nomeProd = vNomeProd) THEN
		INSERT INTO tblProduto (codProduto, nomeProd, dataFab, dataValidade, precoUnitario, qtdEstoque, descCurta, descDetalhada, peso, fotoProd, no_carrinho, destaques, codCategoria) values (default, vNomeProd, vDFab, vDVal, vPUni, vQtd, vDCurta, vDDet, vPeso, vFProd, vCarrinho, vDestaques, (select codCategoria from tblCategoria where codCategoria = vCodCat));
	END IF;
END;
$$

/*
------------------ Bebidas ------------------
call ipProduto("Café Natural", "2024-11-15", "2024-12-15", 28.00, 50, "Café 100% Natural!", "O Pacote de Café Natural contém grãos 100% selecionados, torrados na medida certa para garantir um sabor encorpado e aroma marcante. Adoçado com eritritol, um adoçante natural sem calorias e de baixo índice glicêmico, é ideal para diabéticos ou para quem busca um estilo de vida saudável. Livre de açúcar refinado, glúten, lactose e conservantes artificiais, oferece uma experiência autêntica e equilibrada a cada xícara.", 50.00, "cafe_natural.png", false, false, 4);
call ipProduto("Chá de Canela e Hibisco", "2024-11-15", "2024-12-15", 23.00, 50, "Chá natural de Canela e Hibísco", "O Chá de Canela com Hibisco é feito com canela em pau e hibisco de alta qualidade e adoçado com eritritol, um adoçante natural sem calorias e de baixo índice glicêmico, ideal para diabéticos. Proporciona um sabor aromático e reconfortante, sem açúcar refinado, glúten, lactose ou conservantes artificiais. Uma escolha saudável e funcional para quem controla a ingestão de açúcar.", 50.00, "cha_canela.png", false, false, 4);
call ipProduto("Chá de Laranja", "2024-11-15", "2024-12-15", 23.00, 50, "Chá natural de Laranja", "O Chá de Laranja é preparado com cascas de laranja desidratadas e adoçado com eritritol, um adoçante natural sem calorias e de baixo índice glicêmico. Rico em vitamina C e com um sabor cítrico equilibrado, é livre de açúcar refinado, glúten, lactose e conservantes artificiais. Ideal para uma bebida leve e saudável.", 23.00, "cha_laranja.png", false, false, 4);
call ipProduto("Chá de Limão", "2024-11-15", "2024-12-15", 23.00, 50, "Chá natural de Limão", "O Chá de Limão é elaborado com limões desidratados e adoçado com eritritol, um adoçante natural que não eleva os níveis de glicose. Rico em vitamina C, possui um sabor cítrico e refrescante. Sem açúcar refinado, glúten, lactose ou conservantes artificiais, é ideal para quem busca uma opção saudável e saborosa.", 23.00, "cha_limao.png", false, false, 4);
call ipProduto("Chá de Morango", "2024-11-15", "2024-12-15", 23.00, 50, "Chá natural de Morango", "O Chá de Morango é feito com pedaços de morango desidratados e adoçado com eritritol, um adoçante de baixo índice glicêmico. Rico em antioxidantes, tem um sabor frutado e delicado, perfeito para quem busca uma bebida saudável e saborosa. Sem açúcar refinado, glúten, lactose ou conservantes artificiais.", 23.00, "cha_morango.png", false, false, 4);
call ipProduto("Chá de Limão e Gengibre", "2024-11-15", "2024-12-15", 23.00, 50, "Chá natural de Limão e Gengibre", "O Chá de Gengibre com Hortelã é preparado com gengibre desidratado e Hortelã e adoçado com eritritol, mantendo o baixo índice glicêmico. Oferece um sabor picante e propriedades digestivas e anti-inflamatórias. Sem açúcar refinado, glúten, lactose ou conservantes artificiais, é ideal para momentos de bem-estar.", 23.00, "cha_gengibre.png", false, false, 4);
call ipProduto("Chá de Maracujá", "2024-11-15", "2024-12-15", 23.00, 50, "Chá natural de Maracujá", "O Chá de Maracujá é feito com pedaços de maracujá desidratados e adoçado com eritritol, um adoçante natural sem calorias. Traz um sabor tropical e relaxante, perfeito para momentos tranquilos. Sem açúcar refinado, glúten, lactose ou conservantes artificiais, é uma opção equilibrada e saudável.", 23.00, "cha_maracuja.png", false, false, 4);
call ipProduto("Chá Verde com Hortelã", "2024-11-15", "2024-12-15", 23.00, 50, "Chá verde natural com Hortelã", "O Chá Verde é feito com folhas selecionadas de Camellia sinensis e adoçado com eritritol, mantendo um baixo índice glicêmico, perfeito para diabéticos. Rico em antioxidantes e com um sabor suave, é livre de açúcar refinado, glúten, lactose e conservantes artificiais, oferecendo saúde e equilíbrio em cada xícara.", 23.00, "cha_verde.png", false, false, 4);
call ipProduto("Refrigerante de Abacaxi com Gengibre", "2024-11-15", "2024-12-15", 7.00, 50, "Refrigerante com baixo teor de açúcar sabor abacaxi com gengibre.", "O Refrigerante de Abacaxi é preparado com suco natural de abacaxi (50%) e adoçado com eritritol, um adoçante natural sem calorias e de baixo índice glicêmico. Refrescante e tropical, é livre de açúcar refinado, glúten, lactose ou conservantes artificiais, oferecendo sabor e saúde em uma bebida", 30.00, "refri_abacaxi.png", false, false, 4);
call ipProduto("Refrigerante de Blueberry", "2024-11-15", "2024-12-15", 7.00, 50, "Refrigerante com baixo teor de açúcar sabor blueberry (mirtilo).", "O Refrigerante de Blueberry é feito com suco natural de blueberry (50%) e adoçado com eritritol, perfeito para diabéticos. Rico em antioxidantes e com sabor frutado, é uma bebida livre de açúcar refinado, glúten, lactose e conservantes artificiais.", 30.00, "refri_blueberry.png", false, false, 4);
call ipProduto("Refrigerante de Frutas Vermelhas", "2024-11-15", "2024-12-15", 7.00, 50, "Refrigerante com baixo teor de açúcar sabor de frutas vermelhas.", "O Refrigerante de Frutas Vermelhas combina sucos naturais de morango, amora e framboesa (50%) e é adoçado com eritritol. Rico em antioxidantes, oferece um sabor equilibrado e efervescente, sem açúcar refinado, glúten, lactose ou conservantes artificiais.", 30.00, "refri_vermelho.png", false, false, 4);
call ipProduto("Refrigerante de laranja", "2024-11-15", "2024-12-15", 7.00, 50, "Refrigerante com baixo teor de açúcar sabor laranja.", "O Refrigerante de Laranja é preparado com suco natural de laranja (50%) e adoçado com eritritol, ideal para diabéticos. Rico em vitamina C, tem um sabor cítrico e refrescante. Livre de açúcar refinado, glúten, lactose ou conservantes artificiais.", 30.00, "refri_laranja.png", false, false, 4);
call ipProduto("Refrigerante de Limão", "2024-11-15", "2024-12-15", 7.00, 50, "Refrigerante com baixo teor de açúcar sabor limão", "O Refrigerante de Limão é feito com suco natural de limão (50%) e adoçado com eritritol, oferecendo um sabor ácido e refrescante. Sem açúcar refinado, glúten, lactose ou conservantes artificiais, é perfeito para dias quentes e saudáveis.", 30.00, "refri_limao.png", false, false, 4);
call ipProduto("Refrigerante de Kiwi", "2024-11-15", "2024-12-15", 7.00, 50, "Refrigerante com baixo teor de açúcar sabor kiwi.", "O Refrigerante de Kiwi é elaborado com suco natural de kiwi (50%) e adoçado com eritritol, mantendo o sabor tropical e exótico. Rico em antioxidantes, é livre de açúcar refinado, glúten, lactose e conservantes artificiais, ideal para momentos de frescor.", 30.00, "refri_kiwi.png", false, false, 4);

------------------ Salgados ------------------
call ipProduto("Salgadinho de Tomate e Manjericão", "2024-11-15", "2024-12-15", 6.00, 50, "Um saco de salgadinho saudável sabor tomate e manjericão.", "O Salgadinho de Tomate com Manjericão é feito com tomates frescos (50%) e temperado com manjericão natural, proporcionando um sabor fresco e aromático. Adoçado com eritritol, um adoçante sem calorias e de baixo índice glicêmico, é ideal para diabéticos. Não contém açúcar refinado, glúten, lactose ou conservantes artificiais, oferecendo uma opção saudável e saborosa para quem busca controlar a ingestão de açúcar", 110.00, "salgadinho_tomate.png", false, false, 3);
call ipProduto("Salgadinho Natural", "2024-11-15", "2024-12-15", 6.00, 50, "Um saco de salgadinho saudável temperado com azeite e ervas", "O Salgadinho Natural é feito com ingredientes frescos e naturais e temperado com azeite de oliva e ervas frescas. Adoçado com eritritol, um adoçante sem calorias e de baixo índice glicêmico, é ideal para diabéticos. Contém farinha de grão-de-bico para uma textura crocante e um toque de vinagre de maçã para realçar o sabor. Sem açúcar refinado, glúten, lactose ou conservantes artificiais, é uma opção saudável e saborosa.", 110.00, "salgadinho_natural.png", false, false, 3);
call ipProduto("Salgadinho de Queijo e Cebola", "2024-11-15", "2024-12-15", 6.00, 50, "Um saco de salgadinho saudável sabor queijo e cebola.", "O Salgadinho de Queijo e Cebola é feito com queijo de alta qualidade e cebola natural, combinados para criar um sabor marcante e delicioso. Adoçado com eritritol, um adoçante sem calorias e de baixo índice glicêmico, ele é ideal para diabéticos. Livre de açúcar refinado, glúten, lactose e conservantes artificiais, oferece uma opção saudável e saborosa para quem precisa controlar a ingestão de açúcar.", 110.00, "salgadinho_cebola.png", false, false, 3);
call ipProduto("Salgadinhos de Ervas Finas", "2024-11-1", "2024-12-15", 6.00, 50, "Um saco de salgadinho saudável sabor de ervas finas selecionadas.", "Os Salgadinho de Ervas Finas são feitos com uma mistura de ervas frescas (50%) e temperados com azeite de oliva. Adoçados com eritritol, um adoçante sem calorias e de baixo índice glicêmico, são ideais para diabéticos. Contêm farinha de grão-de-bico para garantir crocância e um toque de vinagre de maçã para realçar o sabor. Sem açúcar refinado, glúten, lactose ou conservantes artificiais, são uma opção saudável e saborosa.", 110.00, "salgadinho_ervas.png", false, false, 3);
call ipProduto("Salgadinho de Alho e Azeite", "2024-11-15", "2024-12-15", 6.00, 50, "Um saco de salgadinho saudável sabor alho e azeite de oliva.", "O Salgadinho de Alho e Azeite é feito com alho fresco (50%) e azeite de oliva extravirgem. Adoçado com eritritol, um adoçante sem calorias e de baixo índice glicêmico, é ideal para diabéticos. Contém farinha de grão-de-bico para uma textura crocante e vinagre de maçã para equilibrar o sabor. Sem açúcar refinado, glúten, lactose ou conservantes artificiais, é uma opção saudável e saborosa.", 110.00, "salgadinho_azeite.png", false, false, 3);
call ipProduto("Salgadinho de Pimenta e Limão", "2024-11-15", "2024-12-15", 6.00, 50, "Um saco de salgadinho saudável sabor limão levemente apimentado.", "O Salgadinho de Pimenta e Limão é feito com pimenta fresca (50%) e temperado com azeite de oliva e limão. Adoçado com eritritol, um adoçante sem calorias e de baixo índice glicêmico, é ideal para diabéticos. Contém farinha de grão-de-bico para uma textura crocante e um toque de vinagre de maçã para realçar o sabor. Sem açúcar refinado, glúten, lactose ou conservantes artificiais, é uma opção saudável e saborosa.", 110.00, "salgadinho_pimenta.png", false, false, 3);
call ipProduto("Bolacha Tradicional", "2024-11-15", "2024-12-15", 7.00, 50, "Bolacha seca sabor tradicional, com farinha de aveia e azeite.", "A Bolacha Tradicional é feita com farinha de aveia ou trigo integral (50%) e temperada com azeite de oliva. Adoçada com eritritol, é ideal para diabéticos. Contém farinha de grão-de-bico para crocância e vinagre de maçã para equilibrar o sabor. Sem açúcar refinado, glúten, lactose ou conservantes artificiais.", 70.00, "bolacha_tradicional.png", false, false, 3);
call ipProduto("Bolacha de Requeijão", "2024-11-15", "2024-12-15", 7.00, 50, "Bolacha seca sabor requeijão cremoso.", "A Bolacha de Requeijão é feita com requeijão cremoso (50%) e temperada com azeite de oliva. Adoçada com eritritol, é ideal para diabéticos. Contém farinha de grão-de-bico para crocância e vinagre de maçã para balancear o sabor. Sem açúcar refinado, glúten, lactose ou conservantes artificiais.", 70.00, "bolacha_requeijao.png", false, false, 3);
call ipProduto("Bolacha de Manteiga", "2024-11-15", "2024-12-15", 7.00, 50, "Bolacha seca sabor manteiga.", "A Bolacha de Manteiga é feita com manteiga de qualidade (50%) e temperada com especiarias naturais. Adoçada com eritritol, é ideal para diabéticos. Contém farinha de grão-de-bico para crocância e vinagre de maçã para realçar o sabor. Sem açúcar refinado, glúten, lactose ou conservantes artificiais.", 70.00, "bolacha_manteiga.png", false, false, 3);
call ipProduto("Bolacha de Cebola", "2024-11-15", "2024-12-15", 7.00, 50, "Bolacha seca sabor cebola", "A Bolacha de Cebola é feita com cebola fresca (50%) e temperada com azeite de oliva. Adoçada com eritritol, é ideal para diabéticos. Contém farinha de grão-de-bico para crocância e vinagre de maçã para equilibrar o sabor. Sem açúcar refinado, glúten, lactose ou conservantes artificiais.", 70.00, "bolacha_cebola.png", false, false, 3);

------------------ Doces ------------------
call ipProduto("Chocolate de Cacau com Amêndoas", "2024-11-15", "2024-12-15", 13.00, 50, "Chocolate levemente amargo sabor amêndoas.", "O Chocolate de Cacau com Amêndoas é uma opção deliciosa e saudável, adoçado com eritritol, um adoçante natural que não eleva os níveis de glicose no sangue. Feito com cacau de alta qualidade, combina o sabor intenso do chocolate com o crocante e nutritivo das amêndoas, oferecendo uma experiência saborosa e rica em nutrientes. Livre de açúcar refinado, lactose, glúten e conservantes artificiais, é perfeito para diabéticos ou para quem busca uma alternativa equilibrada, sem abrir mão do prazer de um chocolate com amêndoas sofisticado e saudável.", 150.00, "chocolate_amendoas.png", false, false, 2);
call ipProduto("Chocolate de Cacau com Frutas vermelhas", "2024-11-15", "2024-12-15", 13.00, 50, "Chocolate levemente amargo sabor frutas vermelhas.", "O Chocolate de Cacau com Frutas Vermelhas é uma opção deliciosa e saudável, adoçado com eritritol, um adoçante natural que não eleva os níveis de glicose no sangue. Feito com cacau de alta qualidade, oferece o sabor intenso do chocolate, combinado com o frescor e a acidez das frutas vermelhas, como morango, framboesa e amora. Livre de açúcar refinado, lactose, glúten e conservantes artificiais, é perfeito para diabéticos ou para quem busca uma alternativa equilibrada, sem abrir mão de um sabor sofisticado e frutado. Ideal para quem deseja um doce saboroso e saudável.", 150.00, "chocolate_frutas.png", false, false, 2);
call ipProduto("Chocolate Branco com Coco", "2024-11-15", "2024-12-15", 13.00, 50, "Chocolate branco com adição de coco.", "O Chocolate Branco com coco é uma opção deliciosa e saudável, adoçado com eritritol, um adoçante natural que não eleva os níveis de glicose no sangue. Feito com manteiga de cacau de alta qualidade, ele oferece a cremosidade característica do chocolate branco, sem adição de açúcar refinado. Livre de lactose, glúten e conservantes artificiais, é perfeito para diabéticos ou para quem busca uma alternativa equilibrada, sem abrir mão do sabor doce e suave do chocolate branco. Ideal para quem deseja um doce saboroso e saudável.", 150.00, "chocolate_branco.png", false, false, 2);
call ipProduto("Geleia de Uva", "2024-11-15", "2024-12-15", 28.00, 50, "Geleia cremosa feita da fruta sabor uva.", "A Geleia de Uva é feita com uvas frescas (50%) e adoçada com eritritol, um adoçante natural sem calorias e de baixo índice glicêmico, ideal para diabéticos. Contém pectina para dar a consistência e suco de limão para equilibrar o sabor. Sem açúcar refinado, glúten, lactose ou conservantes artificiais, oferece uma opção saudável e saborosa para quem controla a ingestão de açúcar.", 100.00, "geleia_uva.png", false, false, 2);
call ipProduto("Geleia de Laranja", "2024-11-15", "2024-12-15", 28.00, 50, "Geleia cremosa feita da fruta sabor laranja.", "A Geleia de Laranja é feita com laranjas frescas (50%) e adoçada com eritritol, um adoçante natural sem calorias e de baixo índice glicêmico, ideal para diabéticos. Contém pectina para dar a consistência e suco de limão para equilibrar o sabor. Sem açúcar refinado, glúten, lactose ou conservantes artificiais, oferece uma opção saudável e saborosa para quem controla a ingestão de açúcar.", 100.00, "geleia_laranja.png", false, false, 2);
call ipProduto("Geleia de Limão", "2024-11-15", "2024-12-15", 28.00, 50, "Geleia cremosa feita da fruta sabor limão.", "A Geleia de Limão é feita com limões frescos (50%) e adoçada com eritritol, um adoçante natural sem calorias e de baixo índice glicêmico, ideal para diabéticos. Contém pectina para dar a consistência e suco de limão para equilibrar o sabor. Sem açúcar refinado, glúten, lactose ou conservantes artificiais, oferece uma opção saudável e saborosa para quem controla a ingestão de açúcar.", 100.00, "geleia_limao.png", false, false, 2);
call ipProduto("Geleia de Morango", "2024-11-15", "2024-12-15", 28.00, 50, "Geleia cremosa feita da fruta sabor morango.", "A geleia de morango é feita com morangos frescos (50%) e adoçada com eritritol, um adoçante natural sem calorias e de baixo índice glicêmico, ideal para diabéticos. Contém pectina para dar a consistência e suco de limão para equilibrar o sabor. Sem açúcar refinado, glúten, lactose ou conservantes artificiais, oferece uma opção saudável e saborosa para quem controla a ingestão de açúcar.", 100.00, "geleia_morango.png", false, false, 2);
call ipProduto("Geleia de Maracujá", "2024-11-15", "2024-12-15", 28.00, 50, "Geleia cremosa feita da fruta sabor maracujá.", "A Geleia de Maracujá é feita com maracujás frescos (50%) e adoçada com eritritol, um adoçante natural sem calorias e de baixo índice glicêmico, ideal para diabéticos. Contém pectina para dar a consistência e suco de limão para equilibrar o sabor. Sem açúcar refinado, glúten, lactose ou conservantes artificiais, oferece uma opção saudável e saborosa para quem controla a ingestão de açúcar.", 100.00, "geleia_maracuja.png", false, false, 2);
call ipProduto("Cookie de Uva", "2024-11-15", "2024-12-15", 7.00, 50, "Biscoito Cookie sem glúten sabor uva.", "O Cookie de Uva é uma opção deliciosa e saudável, feito com uvas desidratadas e adoçado com eritritol, um adoçante natural que não eleva os níveis de glicose no sangue. Preparado com farinha integral ou sem glúten, o cookie oferece uma textura crocante e um sabor suave, com pedaços de uva que adicionam frescor. Sem açúcar refinado, lactose, glúten ou conservantes artificiais, é uma alternativa perfeita para diabéticos ou para quem busca um lanche saboroso e equilibrado.", 75.00, "cookie_uva.png", false, false, 2);
call ipProduto("Cookie de Cereja", "2024-11-15", "2024-12-15", 7.00, 50, "Biscoito Cookie sem glúten sabor cereja.", "O Cookie de Cereja é uma opção deliciosa e saudável, feito com cerejas desidratadas e adoçado com eritritol, um adoçante natural que não eleva os níveis de glicose no sangue. Preparado com farinha integral ou sem glúten, o cookie possui uma textura crocante e um sabor doce e frutado, com pedaços de cereja que adicionam um toque de frescor. Sem açúcar refinado, lactose, glúten ou conservantes artificiais, é ideal para diabéticos ou para quem busca um lanche saboroso e equilibrado.", 75.00, "cookie_cereja.png", false, false, 2);
call ipProduto("Cookie de Baunilha", "2024-11-15", "2024-12-15", 7.00, 50, "Biscoito Cookie sem glúten sabor baunilha.", "O Cookie de Baunilha é uma opção deliciosa e saudável, adoçado com eritritol, um adoçante natural que não afeta os níveis de glicose no sangue. Feito com farinha integral ou sem glúten, o cookie apresenta uma textura crocante e o delicioso sabor suave de baunilha. Livre de açúcar refinado, lactose, glúten e conservantes artificiais, é perfeito para diabéticos ou para quem busca um lanche equilibrado, sem abrir mão do sabor.", 75.00, "cookie_baunilia.png", false, false, 2);
call ipProduto("Cookie de Blueberry", "2024-11-15", "2024-12-15", 7.00, 50, "Biscoito Cookie sem glúten sabor blueberry (mirtilo).", "O Cookie de Blueberry é uma opção deliciosa e saudável, feito com mirtilos desidratados e adoçado com eritritol, um adoçante natural que não eleva os níveis de glicose no sangue. Preparado com farinha integral ou sem glúten, o cookie oferece uma textura crocante e um sabor doce e frutado, com pedaços de blueberry que adicionam frescor. Sem açúcar refinado, lactose, glúten ou conservantes artificiais, é ideal para diabéticos ou para quem busca um lanche equilibrado e saboroso.", 75.00, "cookie_blueberry.png", false, false, 2);
call ipProduto("Cookie de Chocolate Branco", "2024-11-15", "2024-12-15", 7.00, 50, "Biscoito Cookie sem glúten sabor chocolate branco.", "O Cookie de Chocolate Branco é uma opção deliciosa e saudável, adoçado com eritritol, um adoçante natural que não eleva os níveis de glicose no sangue. Feito com farinha integral ou sem glúten, o cookie possui uma textura crocante e o sabor doce e cremoso do chocolate branco, sem comprometer a saúde. Livre de açúcar refinado, lactose, glúten e conservantes artificiais, é uma alternativa perfeita para diabéticos ou para quem busca um lanche saboroso e equilibrado.", 75.00, "cookie_branco.png", false, false, 2);

------------------ Massas ------------------
call ipProduto("Mini Panquecas", "2024-11-15", "2024-12-15", 10.00, 50, "Pequenas e suaves panquecas com um leve sabor de baunília.", "As Mini Panquecas são feitas com ingredientes naturais (50%) e adoçadas com eritritol, um adoçante sem calorias e de baixo índice glicêmico, ideal para diabéticos. Contêm farinha de aveia para uma textura macia e são realçadas com um toque de baunilha. Sem açúcar refinado, glúten, lactose ou conservantes artificiais, são uma opção saudável e saborosa.", 60.00, "panqueca.png", false, false, 1);
call ipProduto("Mini Pães de Queijo", "2024-11-15", "2024-12-15", 23.00, 50, "Pequenos pães de queijo com realce de vinagre de maçã.", "Os Mini Pães de Queijo são feitos com queijo de alta qualidade (50%) e temperados com azeite de oliva. Adoçados com eritritol, são ideais para diabéticos. Contêm farinha de grão-de-bico para uma textura crocante e um toque de vinagre de maçã para equilibrar o sabor. Sem açúcar refinado, glúten, lactose ou conservantes artificiais.", 60.00, "pao_de_queijo.png", false, false, 1);
call ipProduto("Mini Waffles", "2024-11-15", "2024-12-15", 13.00, 50, "Pequenos waffles macios e com leve sabor de baunília.", "Os Mini Waffles são feitos com ingredientes naturais (50%) e adoçados com eritritol, um adoçante sem calorias e de baixo índice glicêmico, ideal para diabéticos. Contêm farinha de aveia para uma textura leve e um toque de baunilha para realçar o sabor. Sem açúcar refinado, glúten, lactose ou conservantes artificiais, são uma opção saudável e deliciosa.", 80.00, "waffle.png", false, false, 1);
call ipProduto("Mini Tortilhas", "2024-11-15", "2024-12-15", 17.00, 50, "Pequenas tortilhas com leve realce de vinagre de maçã.", "As Mini Tortinhas são feitas com ingredientes naturais (50%) e recheadas com opções saudáveis, como frutas frescas ou queijos. Adoçadas com eritritol, são ideais para diabéticos. Contêm farinha de grão-de-bico para crocância e um toque de vinagre de maçã para equilibrar o sabor. Sem açúcar refinado, glúten, lactose ou conservantes artificiais, são uma opção saudável e saborosa.", 80.00, "torta.png", false, false, 1);



select * from tblProduto
select nomeProd, precoUnitario, fotoProd from tblProduto

delete from tblProduto
*/

/*
insert into tblCategoria values (default, 'Massas');
insert into tblCategoria values (default, 'Doces');
insert into tblCategoria values (default, 'Salgados');
insert into tblCategoria values (default, 'Bebidas');

select * from tblCategoria;

*/

/*------------------------------------------------------------------------------------------CARTÃO---------------------------------------------------------------------------------*/

delimiter $$
CREATE PROCEDURE ipCartao(vCodCart decimal(16,0), vNome varchar(150), vTipo bit, vCVV decimal(3,0), vData date)
BEGIN
	INSERT INTO tblCartao (codCartao, idUsu, nomeTitular, tipoCartao, CVV, dataValidade) VALUES (vCodCart, (SELECT idUsu from tblCliente WHERE nomeCompleto = vNome), vNome, vTipo, vCVV, vData);
END;
$$

/*------------------------------------------------------------------------------------------Atualizar Dados---------------------------------------------------------------------------------*/

delimiter $$
CREATE PROCEDURE upCliente(vEmail varchar(150), vSenha varchar(150), vCpf decimal(11,0), vCep decimal(8,0), vLogradouro varchar(200), vBairro varchar(200), vNumCasa int, vComplemento varchar(100), vNUsu varchar(150), vNCUsu varchar(200), vMedica varchar(100), vNasc date, vTel varchar(11), vFoto varchar(200))
BEGIN

	SET @Id = (SELECT idUsu from tblCliente where cpf = vCpf);
    
    IF NOT EXISTS(SELECT cep from tblEndereco where cep = vCep) THEN
	
		IF NOT EXISTS (SELECT bairro from tblBairro where bairro = vBairro) THEN
		
			INSERT INTO tblBairro (bairro) values (vBairro);
		
		END IF;

		INSERT INTO tblEndereco (cep, Logradouro, idBairro) values (vCep, vLogradouro, (SELECT idBairro from tblBairro where bairro = vBairro));

	END IF;
	
    UPDATE tblCliente set cpf=vCpf, cep=vCep, numResidencia=vNumCasa, complemento=vComplemento, email=vEmail, nomeUsu=vNUsu, nomeCompleto=vNCusu, avaliacaoMedica=vMedica, dataNasc=vNasc, tel=vTel, foto=vFoto, senha=vSenha Where idUsu=@Id;
    
    END;
$$
