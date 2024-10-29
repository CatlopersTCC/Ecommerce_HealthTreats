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
    tel varchar(9) not null,
    foto varchar(200) null,
    senha varchar(150) not null,
    nivelAcesso boolean not null
);

/*
select * from tblCliente;
select * from tblLogin;

insert into tblCliente (cpf, idLogin, nomeUsu, nomeCompleto, avaliacaoMedica, dataNasc, tel)
	values (11111111111, 2, "Gama", "Matheus Gama Russi", "Obesidade Tipo 1", "2006-07-06", 911112222);
    
insert into tblLogin
	values (default, "matheus@gmail.com", "1234", 1);
    
delete from tblCliente where idUsu = 2;
delete from tblLogin where idLogin = 1;
*/

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
    fotoProd varchar(200) 
);

create table tblPedido(
	codPedido int primary key auto_increment,
    CPF decimal(11,0), /*FK*/
    idCarrinho int, /*FK*/
    valorTotal decimal(11,2) not null,
    dataHoraCompra datetime not null,
	estimativaEntrega date not null,
    frete decimal(5,2) not null
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

alter table tblPedido add constraint fk_pedido_cliente_cpf foreign key (cpf) references tblCliente(cpf);
alter table tblPedido add constraint fk_pedido_carrinho_idcarrinho foreign key (idCarrinho) references tblCarrinhoCompras(idCarrinho);

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
ipCliente(vEmail varchar(150), vSenha varchar(150), vCpf decimal(11,0), vCep decimal(8,0), vLogradouro varchar(200), vBairro varchar(200), vNumCasa int, vComplemento varchar(100), vNUsu varchar(150), vNCUsu varchar(200), vMedica varchar(100), vNasc date, vTel varchar(9), vFoto varchar(200))
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
select * from tblCliente;
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
*/

delimiter $$
CREATE PROCEDURE
ipProduto(vNomeProd varchar(150), vDFab datetime, vDVal date, vPUni decimal(7,2), vQtd int, vDCurta varchar(200), vDDet text, vPeso decimal(5,2), vFProd varchar(200))
BEGIN
	IF NOT EXISTS (SELECT codProduto from tblProduto where nomeProd = vNomeProd) THEN
		INSERT INTO tblProduto (codProduto, nomeProd, dataFab, dataValidade, precoUnitario, qtdEstoque, descCurta, descDetalhada, peso, fotoProd) values (default, vNomeProd, vDFab, vDVal, vPUni, vQtd, vDCurta, vDDet, vPeso, vFProd);
	END IF;
END;
$$

