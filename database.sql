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
    codProduto int /*FK*/
);

create table tblPagamento(
	idPagamento int primary key auto_increment,
    cpf decimal(11,0), /*FK*/
    formaPag varchar(50) not null,
    dataHoraPag datetime not null
);

create table tblAvaliacao(
	idAvaliacao int primary key auto_increment,
    idUsu int, /*FK*/
    qtdEstrela decimal(5,0) not null,
    comentario text not null,
    imagensProd longblob
);

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

alter table tblPagamento add constraint fk_pagamento_cliente_idusu foreign key (cpf) references tblCliente(cpf);

alter table tblAvaliacao add constraint fk_avaliacao_cliente_idusu foreign key (idUsu) references tblCliente(idUsu);

alter table tblEndereco add constraint fk_endereco_bairro_idbairro foreign key (idBairro) references tblBairro(idBairro);

alter table tblCartao add constraint fk_cartao_usuario_idusu foreign key (idUsu) references tblCliente(idUsu);




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
call ipProduto("Chá de Limão", "2024-11-15", "2024-12-15", 28.00, 50, "Chá natural de Limão", "O Chá de Limão é elaborado com limões desidratados e adoçado com eritritol, um adoçante natural que não eleva os níveis de glicose. Rico em vitamina C, possui um sabor cítrico e refrescante. Sem açúcar refinado, glúten, lactose ou conservantes artificiais, é ideal para quem busca uma opção saudável e saborosa.", 23.00, "cha_limao.png", false, false, 4);
call ipProduto("Chá de Morango", "2024-11-15", "2024-12-15", 28.00, 50, "Chá natural de Morango", "O Chá de Morango é feito com pedaços de morango desidratados e adoçado com eritritol, um adoçante de baixo índice glicêmico. Rico em antioxidantes, tem um sabor frutado e delicado, perfeito para quem busca uma bebida saudável e saborosa. Sem açúcar refinado, glúten, lactose ou conservantes artificiais.", 23.00, "cha_morango.png", false, false, 4);
call ipProduto("Chá de Limão e Gengibre", "2024-11-15", "2024-12-15", 28.00, 50, "Chá natural de Limão e Gengibre", "O Chá de Gengibre com Hortelã é preparado com gengibre desidratado e Hortelã e adoçado com eritritol, mantendo o baixo índice glicêmico. Oferece um sabor picante e propriedades digestivas e anti-inflamatórias. Sem açúcar refinado, glúten, lactose ou conservantes artificiais, é ideal para momentos de bem-estar.", 23.00, "cha_gengibre.png", false, false, 4);
call ipProduto("Chá de Maracujá", "2024-11-15", "2024-12-15", 28.00, 50, "Chá natural de Maracujá", "O Chá de Maracujá é feito com pedaços de maracujá desidratados e adoçado com eritritol, um adoçante natural sem calorias. Traz um sabor tropical e relaxante, perfeito para momentos tranquilos. Sem açúcar refinado, glúten, lactose ou conservantes artificiais, é uma opção equilibrada e saudável.", 23.00, "cha_maracuja.png", false, false, 4);
call ipProduto("Chá Verde com Hortelã", "2024-11-15", "2024-12-15", 28.00, 50, "Chá verde natural com Hortelã", "O Chá Verde é feito com folhas selecionadas de Camellia sinensis e adoçado com eritritol, mantendo um baixo índice glicêmico, perfeito para diabéticos. Rico em antioxidantes e com um sabor suave, é livre de açúcar refinado, glúten, lactose e conservantes artificiais, oferecendo saúde e equilíbrio em cada xícara.", 23.00, "cha_verde.png", false, false, 4);
call ipProduto("", "2024-11-15", "2024-12-15", 28.00, 50, "", "", 7.00, ".png", false, false, 1);
call ipProduto("", "2024-11-15", "2024-12-15", 28.00, 50, "", "", 7.00, ".png", false, false, 1);
call ipProduto("", "2024-11-15", "2024-12-15", 28.00, 50, "", "", 7.00, ".png", false, false, 1);
call ipProduto("", "2024-11-15", "2024-12-15", 28.00, 50, "", "", 7.00, ".png", false, false, 1);
call ipProduto("", "2024-11-15", "2024-12-15", 28.00, 50, "", "", 7.00, ".png", false, false, 1);
call ipProduto("", "2024-11-15", "2024-12-15", 28.00, 50, "", "", 7.00, ".png", false, false, 1);




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
	IF NOT EXISTS(SELECT codCartao from tblCartao where nomeTitular = vNome) THEN
		INSERT INTO tblCartao (codCartao, idUsu, nomeTitular, tipoCartao, CVV, dataValidade) VALUES (vCodCart, (SELECT idUsu from tblCliente WHERE nomeCompleto = vNome), vNome, vTipo, vCVV, vData);
	END IF;
END;
$$
