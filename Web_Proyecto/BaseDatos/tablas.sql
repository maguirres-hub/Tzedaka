create table
    if not exists paises(
        id_pais int auto_increment,
        pais varchar(64) null,
        primary key(id_pais)
    ) ENGINE = InnoDB default charset = utf8mb4;

create table
    if not exists ciudades(
        id_ciudad int auto_increment,
        ciudad varchar(64) null,
        id_pais int null,
        primary key(id_ciudad),
        Foreign Key (id_pais) REFERENCES paises(id_pais)
    ) ENGINE = InnoDB default charset = utf8mb4;

create table
    if not exists cursos(
        id_curso int auto_increment,
        nombre_curso varchar(100) null,
        descripcion varchar(100) null,
        url_video varchar(256) null,
        primary key(id_curso)
    ) ENGINE = InnoDB default charset = utf8mb4;

create table
    if not exists comentarios(
        id_comentario int auto_increment,
        comentario varchar(256) null,
        id_curso int null,
        id_cliente int null,
        primary key(id_comentario),
        foreign key(id_curso) references cursos(id_curso),
        foreign key(id_cliente) references clientes(id_cliente)
    ) Engine = InnoDB default charset = utf8mb4;

create table
    if not exists subscripciones(
        id_subscripcion int auto_increment,
        id_detalles_sub int null,
        id_cliente int UNIQUE null,
        fecha_inicio date null,
        fecha_final date null,
        bloque int null,
        posicion_bloque int null,
        ultima_donacion float(0),
        vueltas int(0),
        donacion_activa TINYINT(0),
        primary key(id_subscripcion),
        Foreign Key (id_detalles_sub) REFERENCES detalles_subscripcion(id_detalles_sub),
        Foreign Key (id_cliente) REFERENCES clientes(id_cliente)
    ) Engine = InnoDB DEFAULT charset = utf8mb4;

create table
    if not exists detalles_subscripcion(
        id_detalles_sub int auto_increment,
        precio float(0) not null,
        primary key(id_detalles_sub)
    ) Engine = InnoDB DEFAULT charset = utf8mb4;

create table
    if not exists cargos(
        id_cargo int auto_increment,
        cargo varchar(54),
        primary key(id_cargo)
    ) ENGINE = InnoDB default charset = utf8mb4;

create table
    if not exists reportes_clientes(
        id_reporte_cliente int auto_increment,
        titulo_reporte varchar(100) null,
        reporte varchar(256) null,
        respuesta varchar(256) null,
        id_cliente int null,
        primary key(id_reporte_cliente),
        Foreign Key (id_cliente) REFERENCES clientes(id_cliente)
    ) ENGINE = InnoDB default charset = utf8mb4;

create table
    if not exists oficinas(
        id_oficina int auto_increment,
        id_pais int null,
        id_ciudad int null,
        telefono varchar(20) null,
        direccion varchar(100) null,
        primary key(id_oficina),
        foreign key (id_pais) references paises(id_pais),
        foreign key (id_ciudad) REFERENCES ciudades(id_ciudad)
    ) ENGINE = InnoDB default charset = utf8mb4;

create table
    if not exists empleados(
        id_empleado int auto_increment,
        nombres varchar(100) null,
        apellidos varchar(100) null,
        correo varchar(100) null UNIQUE,
        id_cargo int null,
        fecha_registroem date null,
        password varchar(100) null,
        active int null,
        primary key(id_empleado),
        foreign key(id_cargo) references cargos(id_cargo)
    ) ENGINE = InnoDB default charset = utf8mb4;

create table
    if not exists clientes(
        id_cliente int auto_increment,
        nombres varchar (100) null,
        apellidos varchar(100) null,
        telefono varchar(20) null,
        direccion varchar(100) null,
        correo varchar(100) null UNIQUE,
        password varchar(128) null,
        id_ciudad int null,
        fecha_registrocl date null,
        active int null,
        primary key(id_cliente),
        foreign key(id_ciudad) references ciudades(id_ciudad)
    ) ENGINE = InnoDB default charset = utf8mb4;

create table
    if not exists estados(
        id_estado int auto_increment,
        beravngj_tzedakin estado varchar(100),
        primary key(id_estado)
    ) ENGINE = InnoDB default charset = utf8mb4;

create table
    if not exists pago(
        id_pago int auto_increment,
        forma_pago varchar(100),
        primary key(id_pago)
    ) ENGINE = InnoDB default charset = utf8mb4;

create table
    if not exists categorias(
        id_categoria int auto_increment,
        categoria varchar(100) null,
        primary key(id_categoria)
    ) ENGINE = InnoDB default charset = utf8mb4;

create table
    if not exists productos(
        id_producto int auto_increment,
        nombre varchar(100) null,
        descripcion varchar(100) null,
        stock int null,
        precio float null,
        id_categoria int null,
        id_estado int null,
        url_imagen int null,
        img_blob LONGBLOB null,
        id_cliente int null;

primary key(id_producto),
foreign key(id_categoria) references categorias(id_categoria),
foreign key(id_estado) references estados(id_estado),
FOREIGN KEY(id_cliente) REFERENCES clientes(id_cliente);

)ENGINE=InnoDB default charset=utf8mb4;

create table
    if not exists detalle_pedido(
        id_detalle int auto_increment,
        cantidad int null,
        precio_unidad float null,
        comentario varchar(100),
        id_pedido int null,
        id_producto int null,
        primary key(id_detalle),
        foreign key(id_pedido) references pedidos(id_pedido),
        foreign key(id_producto) references productos(id_producto)
    ) ENGINE = InnoDB default charset = utf8mb4;

create table
    if not exists billetera_virtual(
        id_billetera int auto_increment,
        id_cliente int null,
        total float(0) null,
        primary key(id_billetera),
        Foreign Key (id_cliente) REFERENCES clientes(id_cliente)
    ) engine = innodb default charset = utf8mb4;

create table
    if not exists reportes_billetera(
        id_reporte_billetera int auto_increment,
        fecha date null,
        cantidad float(0) null,
        motivo varchar(100) null,
        codigo_producto int null,
        total float(0) null,
        id_cliente int null,
        primary key(id_reporte_billetera),
        Foreign Key (id_cliente) REFERENCES clientes(id_cliente)
    ) engine = innodb default charset = utf8mb4;

    create table
    if not exists reportes_retiros(
        id_reporte_retiro int auto_increment,
        fecha date null,
        cantidad float(0) null,
        id_estado int null,
        id_cliente int null,
        descripcion varchar(256),
        primary key(id_reporte_retiro),
        Foreign Key (id_estado) REFERENCES estados(id_estado),
        Foreign Key (id_cliente) REFERENCES clientes(id_cliente)
    ) engine = innodb default charset = utf8mb4;
create table
    if not exists solicitudes_subs(
        id_solicitud int auto_increment,
        id_cliente int null,
        activado int null,
        primary key(id_solicitud),
        Foreign Key (id_cliente) REFERENCES clientes(id_cliente)
    ) engine = innodb default charset = utf8mb4;
insert into detalles_subscripcion (precio)values(0.00);

insert into estados (estado)values('pendiente'),('aprobado');

insert into pago (forma_pago)
values
('Tarjeta de debito'), ('Tarjeta de credito'), ('Transferencia Bancaria');

insert into
    categorias (categoria)
values
('ropa'), ('comida'), ('electronica');

insert into cargos (cargo)values("administrador"),("moderador");

insert into paises (pais) values ('Colombia');

insert into
    ciudades (ciudad, id_pais)
values ('Medellin', 1), ('Cali', 1), ('Bogota', 1);