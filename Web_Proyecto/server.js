var mysql = require('mysql');
var express = require('express');
var cors = require('cors');
var nodemailer = require('nodemailer');


app = express();
app.use(express.json());
function conectar() {
    const con = mysql.createConnection({
        host: "127.0.0.1",
        user: "tzedgvuf_root",
        password: "Contraseña1",
        database: "tzedgvuf_tzedakin"
    });
    return con;
}
app.use(cors());

// ------------------------------------- correo ------------------------------//
app.post('/tzedakin/api/correo_rg_gmail', (req, res) => {
    const { from, to, subject, text } = req.body;
    const envio = nodemailer.createTransport({
        host: "smtp.dondominio.com",
        port: 587,
        secure: false,

        auth: {

            user: "berajot@jorgehernandezr.dev",
            pass: "V6lrMvXdZJ[Z"
        }
    });
    const mailOpciones = {
        from: from,
        to: to,
        subject: subject,
        text: text
    };
    envio.sendMail(mailOpciones, function (error, info) {
        if (error) {
            console.log(error);
            res.status(500).send('Error al enviar el correo electrónico: ' + error);
        } else {
            console.log('Correo electrónico enviado: ' + info.response);
            res.status(200).send('Correo electrónico enviado correctamente');
        }
    });
});



// ------------------------------------- pais ---------------------//
app.get('/tzedakin/api/paises/', (req, res) => {
    const con = conectar();
    con.connect((error) => {
        if (error) {
            res.send(err);
        } else {
            con.query("select * from paises", function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    })
});
app.post('/tzedakin/api/paises/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            param = [req.body.pais]
            con.query("insert into paises(pais) values (?)", [param], function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.put('/tzedakin/api/paises/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.pais, req.params.id];
            con.query("update paises set pais = ? where id_pais = ?", param, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.delete('/tzedakin/api/paises/:id', (req, res) => {
    const idPais = req.params.id;

    // Iniciar la conexión a la base de datos
    const con = conectar();
    // Eliminar el país
    const deletePaisQuery = 'DELETE FROM paises WHERE id_pais = ?';
    con.query(deletePaisQuery, [idPais], (err, result) => {
        if (err) {
            res.send(err);
        } else {
            res.send(result);
        }
        con.end();
    });
});



// ------------------------------------- Ciudades ---------------------------//

app.get('/tzedakin/api/ciudades/', (req, res) => {
    const con = conectar();
    con.connect((error) => {
        if (error) {
            res.send(err);
        } else {
            con.query("select * from ciudades", function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    })
});
app.get('/tzedakin/api/ciudades/:id', (req, res) => {
    const con = conectar();
    con.connect((error) => {
        if (error) {
            res.send(err);
        } else {
            con.query("select * from ciudades where id_ciudad = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    })
});
app.get('/tzedakin/api/ciudades_inner_pais/', (req, res) => {
    const con = conectar();
    con.connect((error) => {
        if (error) {
            res.send(err);
        } else {
            con.query("select ci.id_ciudad, ci.ciudad, pa.id_pais, pa.pais from ciudades ci join paises pa on ci.id_pais = pa.id_pais", function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    })
});
app.get('/tzedakin/api/ciudades_inner_pais/:id', (req, res) => {
    const con = conectar();
    con.connect((error) => {
        if (error) {
            res.send(err);
        } else {
            con.query("select ci.id_ciudad, ci.ciudad from ciudades ci join paises pa on ci.id_pais = pa.id_pais where ci.id_pais = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    })
});
app.post('/tzedakin/api/ciudades/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            param = [req.body.id_pais, req.body.ciudad];
            con.query("insert into ciudades(id_pais, ciudad) values (?)", [param], function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.put('/tzedakin/api/ciudades/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.ciudad, req.body.id_pais, req.params.id];
            con.query("update ciudades set ciudad = ?, id_pais = ? where id_ciudad = ?", param, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });
        }
    });
});
app.delete('/tzedakin/api/ciudades/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("delete from ciudades where id_ciudad = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
// ------------------------------------- estados -----------------------------//
app.get('/tzedakin/api/estados/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from estados", function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(JSON.stringify(result));
                }
                con.end();
            });

        }
    });
});
app.get('/tzedakin/api/estados/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from estados where id_estado = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(JSON.stringify(result));
                }
                con.end();
            });

        }
    });
});
app.post('/tzedakin/api/estados/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            param = [req.body.estado]
            con.query("insert into estados(estado) values (?)", [param], function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(JSON.stringify(result));
                }
                con.end();
            });

        }
    });
});
app.put('/tzedakin/api/estados/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.estado];
            con.query("update estados set estado = ? where id_estado = ?", param, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(JSON.stringify(result));
                }
                con.end();
            });

        }
    });
});
app.delete('/tzedakin/api/estados/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("delete from estado where id_estado = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(JSON.stringify(result));
                }
                con.end();
            });

        }
    });
});
//---------------------------------- empleados -------------------------------------
app.get('/tzedakin/api/login_empleados/:email/:pass', (req, res) => {
    const con = conectar();
    let sql = 'SELECT count(1) is_valid FROM empleados where correo = ? and password = ?'; /* and active = 1 */
    let parametros = [
        req.params.email,
        req.params.pass
    ];

    con.connect(function (err) {

        if (err) {
            res.send(err);
        } else {
            con.query(sql, parametros, function (err, result) {

                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });

});
app.get('/tzedakin/api/empleados/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from empleados", function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(JSON.stringify(result));
                }
                con.end();
            });

        }
    });
});
app.get('/tzedakin/api/empleados/:id', (req, res) => {

    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from empleados where id_empleado = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(JSON.stringify(result));
                }
                con.end();
            });

        }
    });
});
app.get('/tzedakin/api/empleado_email/:email', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from empleados em left join cargos c on em.id_cargo = c.id_cargo where correo = ?", req.params.email, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(JSON.stringify(result));
                }
                con.end();
            });

        }
    });
});
app.post('/tzedakin/api/empleados/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.nombres, req.body.apellidos, req.body.correo, req.body.id_cargo, req.body.fecha_registroem, req.body.password, req.body.active];
            con.query("insert into empleados (nombres, apellidos, correo, id_cargo, fecha_registroem, password, active) values (?)", [param], function (err, result) {
                if (err) {
                    res.send("Correo_Encontrado");
                } else {
                    res.send(JSON.stringify(result.affectedRows));
                }
                con.end();
            });

        }
    });
});
app.put('/tzedakin/api/empleados/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.nombres, req.body.apellidos, req.body.correo, req.body.id_cargo, req.body.fecha_registroem, req.body.password, req.body.active, req.params.id];
            con.query("update empleados set nombres = ?, apellidos = ?, correo = ?, id_cargo = ?, fecha_registroem = ?, password = ?, active = ? where id_empleado = ?", param, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(JSON.stringify(result));
                }
                con.end();
            });

        }
    });
});
app.delete('/tzedakin/api/empleados/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("delete from empleados where id_empleado = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(JSON.stringify(result));
                }
                con.end();
            });

        }
    });
});
// --------------------------------------------------- clientes ---------------------------
app.get('/tzedakin/api/login_clientes/:email/:pass', (req, res) => {
    const con = conectar();
    let sql = 'SELECT count(1) as is_valid, CASE WHEN cl.active IS NULL THEN 0 ELSE 1 END As active, CASE WHEN subs.id_subscripcion IS NULL THEN 0 ELSE 1 END AS id_subscripcion FROM clientes cl left join subscripciones subs on cl.id_cliente = subs.id_cliente where correo = ? and password = ?'; /* and active = 1 */
    let parametros = [
        req.params.email,
        req.params.pass
    ];

    con.connect(function (err) {

        if (err) {
            res.send(err);
        } else {
            con.query(sql, parametros, function (err, result) {

                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});

app.get('/tzedakin/api/clientes/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from clientes where id_cliente = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.get('/tzedakin/api/clientes/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from clientes", function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.get('/tzedakin/api/cliente_email/:email', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("SELECT cl.*, c.ciudad,p.id_pais, p.pais, subs.bloque, CASE WHEN subs.id_subscripcion IS NULL THEN 0 ELSE subs.id_subscripcion END as id_subscripcion, CASE WHEN subs.bloque is null then 0 else subs.bloque end as bloque FROM clientes cl LEFT JOIN ciudades c ON cl.id_ciudad = c.id_ciudad LEFT JOIN paises p ON p.id_pais = c.id_pais LEFT JOIN billetera_virtual wallet ON cl.id_cliente = wallet.id_cliente LEFT JOIN subscripciones subs ON cl.id_cliente = subs.id_cliente WHERE cl.correo = ?", req.params.email, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.post('/tzedakin/api/clientes/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.nombres, req.body.apellidos, req.body.telefono, req.body.direccion, req.body.correo, req.body.password, req.body.id_ciudad, req.body.fecha_registrocl, req.body.active];
            con.query("insert into clientes (nombres, apellidos, telefono, direccion, correo, password, id_ciudad, fecha_registrocl, active) values (?)", [param], function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.put('/tzedakin/api/clientes/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.nombres, req.body.apellidos, req.body.telefono, req.body.direccion, req.body.correo, req.body.password, req.body.id_ciudad, req.params.id];
            con.query("update clientes set nombres = ?, apellidos = ?, telefono = ?, direccion = ?, correo = ?, password = ?, id_ciudad = ? where id_cliente = ?", param, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(JSON.stringify(result));
                }
                con.end();
            });

        }
    });
});
app.put('/tzedakin/api/clientes_actualizacion_app/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.nombres, req.body.apellidos, req.body.telefono, req.body.direccion, req.body.password, req.body.id_ciudad, req.params.id];
            con.query("update clientes set nombres = ?, apellidos = ?, telefono = ?, direccion = ?,  password = ?, id_ciudad = ? where id_cliente = ?", param, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(JSON.stringify(result));
                }
                con.end();
            });

        }
    });
});
app.put('/tzedakin/api/clientes_estatus/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.active, req.params.id];
            con.query("update clientes set active = ? where id_cliente = ?", param, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.put('/tzedakin/api/recuperar_password/:correo', (req, res) => {
    const correo = req.params.correo;
    const password = req.body.password;

    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            // Consulta para verificar si hay un usuario con el correo
            con.query("SELECT * FROM clientes WHERE correo = ?", correo, function (err, results) {
                if (err) {
                    res.send(err);
                } else {
                    if (results.length === 0) {
                        // No se encontró ningún usuario con el correo proporcionado
                        res.status(404).send("No se encontró ningún usuario con el correo proporcionado.");
                    } else {
                        // Actualizar la contraseña del usuario
                        let param = [password, correo];
                        con.query("UPDATE clientes SET password = ? WHERE correo = ?", param, function (err, result) {
                            if (err) {
                                res.send(err);
                            } else {
                                res.send(result);
                            }
                        });
                    }
                }
                con.end();
            });
        }
    });
});
app.delete('/tzedakin/api/clientes/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("delete from clientes where id_cliente = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    }); // delete from clientes where id_cliente = ?; probar doble params
});
app.delete('/tzedakin/api/clientes_inner_delete/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("DELETE wallet, subs, rep, cl FROM clientes cl join billetera_virtual wallet on cl.id_cliente = wallet.id_cliente left join subscripciones subs on subs.id_cliente = cl.id_cliente left join reportes_billetera rep on rep.id_cliente = cl.id_cliente left join productos pro on pro.id_cliente = cl.id_cliente where cl.id_cliente = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });
        }
    }); // delete from clientes where id_cliente = ?;
});
app.get('/tzedakin/api/clientes_inner/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select cl.*, ci.*, subs.*, wallet.* from clientes cl left join ciudades ci on cl.id_ciudad = ci.id_ciudad left join subscripciones subs on subs.id_cliente = cl.id_cliente left join billetera_virtual wallet on wallet.id_cliente = cl.id_cliente  where cl.id_cliente = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });
        }
    });
});
app.get('/tzedakin/api/clientes_inner/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select cl.*, ci.*, subs.*, wallet.* from clientes cl left join ciudades ci on cl.id_ciudad = ci.id_ciudad left join subscripciones subs on subs.id_cliente = cl.id_cliente left join billetera_virtual wallet on wallet.id_cliente = cl.id_cliente", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.get('/tzedakin/api/clientes_InnerCiudad/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from clientes cl left join ciudades ci on cl.id_ciudad = ci.id_ciudad where id_cliente = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.get('/tzedakin/api/clientes_InnerCiudad/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from clientes cl left join ciudades ci on cl.id_ciudad = ci.id_ciudad", function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
// ----------------------------------- billetera virtual ------------------------------//
app.get('/tzedakin/api/billetera_virtual_cliente/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from billetera_virtual where id_cliente = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);

                }
                con.end();
            });

        }
    });
});
app.get('/tzedakin/api/billetera_virtual/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from billetera_virtual where id_cliente = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);

                }
                con.end();
            });

        }
    });
});
app.get('/tzedakin/api/billetera_virtual_inner/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from billetera_virtual where id_billetera = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.get('/tzedakin/api/billetera_virtual/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from billetera_virtual", function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.post('/tzedakin/api/billetera_virtual/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.id_cliente, req.body.total];
            con.query("insert into billetera_virtual (id_cliente, total) values (?)", [param], function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.put('/tzedakin/api/billetera_virtual/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.total,req.body.creditos,req.params.id];
            con.query("update billetera_virtual set total = ? , creditos = ? where id_cliente = ?", param, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.delete('/tzedakin/api/billetera_virtual/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("delete from billetera_virtual where id_billetera = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
// ----------------------------------- reportes billetera virtual ------------------------------//
app.get('/tzedakin/api/reportes_billetera/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from reportes_billetera where id_reporte_billetera = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);

                }
                con.end();
            });

        }
    });
});
app.get('/tzedakin/api/reportes_billetera/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from reportes_billetera", function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.get('/tzedakin/api/reportes_billetera_inner/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select rep_wallet.* from reportes_billetera rep_wallet left join clientes cl on rep_wallet.id_cliente = cl.id_cliente WHERE cl.id_cliente = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.post('/tzedakin/api/reportes_billetera/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.fecha, req.body.cantidad, req.body.total, req.body.id_cliente, req.body.motivo, req.body.codigo_producto];
            con.query("insert into reportes_billetera (fecha, cantidad, total, id_cliente, motivo, codigo_producto) values (?)", [param], function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.post('/tzedakin/api/reportes_retiro/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.fecha, req.body.cantidad, req.body.id_estado, req.body.id_cliente, req.body.descripcion];
            con.query("insert into reportes_retiros (fecha, cantidad, id_estado, id_cliente, descripcion) values (?)", [param], function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.put('/tzedakin/api/reportes_billetera/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.fecha, req.body.cantidad, req.body.total, req.body.motivo, req.body.codigo_producto, req.params.id];
            con.query("update reportes_billetera set fecha_carga = ?, cantidad_carga = ?, fecha_retiro = ?, cantidad_retiro = ?, total = ? where id_reporte_billetera = ?", param, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });
        }
    });
});
app.put('/tzedakin/api/reportes_billetera_carga/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.fecha_carga, req.body.cantidad_carga, req.params.id];
            con.query("update reportes_billetera set fecha_carga = ?, cantidad_carga = ? where id_reporte_billetera = ?", param, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });
        }
    });
});
app.put('/tzedakin/api/reportes_billetera_retiro/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.fecha_retiro, req.body.cantidad_retiro, req.params.id];
            con.query("update reportes_billetera set fecha_retiro = ?, cantidad_retiro = ? where id_reporte_billetera = ?", param, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });
        }
    });
});
app.delete('/tzedakin/api/reportes_billetera/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("delete from reportes_billetera where id_reporte_billetera = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
// ----------------------------------- reporte clientes ----------------------///
app.get('/tzedakin/api/reportes_clientes/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from reportes_cliente where id_reporte_cliente = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);

                }
                con.end();
            });

        }
    });
});
app.get('/tzedakin/api/reportes_clientes/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from reportes_clientes", function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.post('/tzedakin/api/reportes_clientes/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.titulo_reporte, req.body.reporte, req.body.respuesta, req.body.id_cliente];
            con.query("insert into reportes_clientes (titulo_reporte, reporte, respuesta, id_cliente) values (?)", [param], function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);

                }
                con.end();
            });

        }
    });
});
app.put('/tzedakin/api/reportes_clientes/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.titulo_reporte, req.body.reporte, req.body.respuesta, req.body.id_cliente, req.params.id];
            con.query("update reportes_clientes set titulo_reporte = ?, reporte = ?, respuesta = ?, id_cliente = ? where id_reporte_cliente = ?", param, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.delete('/tzedakin/api/reportes_clientes/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("delete from reportes_clientes where id_reporte_cliente = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
// ------------------------------------ cursos -------------------------------//
app.get('/tzedakin/api/cursos/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from cursos where id_curso = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.get('/tzedakin/api/cursos/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from cursos", function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.post('/tzedakin/api/cursos/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.nombre_curso, req.body.descripcion, req.body.url_video];
            con.query("insert into cursos (nombre_curso, descripcion, url_video) values (?)", [param], function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.put('/tzedakin/api/cursos/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.nombre_curso, req.body.descripcion, req.body.url_video];
            con.query("update cursos set nombre_curso = ?, descripcion = ?, url_video = ? where id_curso = ?", param, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.delete('/tzedakin/api/cursos/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("delete from cursos where id_curso = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
// ------------------------------------ comentarios -------------------------------//
app.get('/tzedakin/api/comentarios/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from comentarios", function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.get('/tzedakin/api/comentarios/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from comentarios where id_comentario = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);

                }
                con.end();
            });

        }
    });
});
app.get('/tzedakin/api/comentarios_inner/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from comentarios com left join cursos cur on com.id_curso = cur.id_curso left join clientes cl on com.id_cliente = cl.id_cliente", function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.post('/tzedakin/api/comentarios/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.comentario, req.body.id_curso, req.body.id_cliente];
            con.query("insert into comentarios (comentario, id_curso, id_cliente) values (?)", [param], function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.put('/tzedakin/api/comentarios/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.comentario, req.body.id_curso, req.body.id_cliente];
            con.query("update comentarios set comentario = ?, id_cliente = ?, id_cliente = ? where id_comentario = ?", param, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.delete('/tzedakin/api/comentarios/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("delete from comentarios where id_comentario = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
// ----------------------------------- pago ----------------------///
app.get('/tzedakin/api/pago/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from pago where id_pago = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);

                }
                con.end();
            });

        }
    });
});
app.get('/tzedakin/api/pago', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from pago", function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.post('/tzedakin/api/pago', (req, res) => {
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.forma_pago, req.body.fecha_pago, req.body.id_pedido];
            con.query("insert into pago (forma_pago, fecha_pago, id_pedido) values (?)", [param], function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.put('/tzedakin/api/pago/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.forma_pago, req.body.fecha_pago, req.body.id_pedido, req.params.id];
            con.query("update pago set forma_pago = ?, fecha_pago = ?, id_pedido = ? where id_pago = ?", param, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.delete('/tzedakin/api/pago/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("delete from pago where id_pago = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
// ------------------------------------ detalles subscripcion ---------------------------
app.get('/tzedakin/api/detalles_subscripcion/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from detalles_subscripcion where id_detalles_sub = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.get('/tzedakin/api/detalles_subscripcion', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from detalles_subscripcion", function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.post('/tzedakin/api/detalles_subscripcion', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.precio];
            con.query("insert into detalles_subscripcion (precio) values (?)", [param], function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(esult);
                }
                con.end();
            });

        }
    });
});
app.put('/tzedakin/api/detalles_subscripcion/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.precio, req.params.id];
            con.query("update detalles_subscripcion set precio = ? where id_detalles_sub = ?", param, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.delete('/tzedakin/api/detalles_subscripcion/:id', (req, res) => {
    const con = conectar();

    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("delete from detalles_subscripcion where id_detalles_sub = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
// ----------------------------------- subscripciones ----------------------///
app.get('/tzedakin/api/subscripciones/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from subscripciones where id_subscripcion = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);

                }
                con.end();
            });
        }
    });
});
app.get('/tzedakin/api/subscripcion_cliente/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from subscripciones where id_cliente= ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);

                }
                con.end();
            });
        }
    });
});
app.get('/tzedakin/api/subscripciones', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from subscripciones", function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});

app.get('/tzedakin/api/subscriptor_vueltas/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select  from subscripciones where id_cliente = ?", req.params.id_cliente, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});

app.get('/tzedakin/api/subscripciones_innner', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select cl.nombres, cl.apellidos, cl.telefono, cl.correo, ciu.ciudad, subs.id_subscripcion, subs.bloque, subs.posicion_bloque, subs.fecha_inicio, subs.fecha_final from subscripciones subs left join detalles_subscripcion d_subs on subs.id_detalles_sub = d_subs.id_detalles_sub left join clientes cl on subs.id_cliente = cl.id_cliente left join ciudades ciu on cl.id_ciudad = ciu.id_ciudad", function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});

app.get('/tzedakin/api/primer_subscriptor', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select cl.id_cliente, cl.nombres, cl.apellidos from clientes cl left join subscripciones subs on subs.id_cliente = cl.id_cliente where subs.bloque = 1 and subs.posicion_bloque = 1;", function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});

app.get('/tzedakin/api/segundo_subscriptor', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select cl.id_cliente, cl.nombres, cl.apellidos from clientes cl left join subscripciones subs on subs.id_cliente = cl.id_cliente where subs.bloque = 1 and subs.posicion_bloque = 1;", function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});


app.post('/tzedakin/api/subscripciones', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [1, req.body.id_cliente, req.body.fecha_inicio, req.body.fecha_final, req.body.bloque, req.body.posicion_bloque, req.body.ultima_donacion, req.body.vueltas, req.body.donacion_activa];
            con.query("insert into subscripciones (id_detalles_sub, id_cliente, fecha_inicio, fecha_final, bloque, posicion_bloque, ultima_donacion, vueltas, donacion_activa) values (?)", [param], function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.put('/tzedakin/api/subscripciones/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.id_detalles_sub, req.body.id_cliente, req.body.fecha_inicio, req.body.fecha_final, req.body.bloque, req.body.posicion_bloque, req.body.ultima_donacion, req.body.vueltas, req.body.donacion_activa, req.params.id];
            con.query("update subscripciones set id_detalles_sub = ?, id_cliente = ?, fecha_inicio = ?, fecha_final = ?, bloque = ?, posicion_bloque = ?, ultima_donacion = ?, vueltas = ?, donacion_activa = ? where id_subscripcion = ?", param, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});

app.put('/tzedakin/api/subscripciones/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let query_subs = "select max(posicion_bloque) as posicion_bloque, max(bloque) as bloque from subscripciones;"
            con.query(query_subs, function(err, result){
                if(err){
                    res.send(err);
                }else{
                    res.send(result);
                }
            })
            /*let param = [req.body.id_detalles_sub, req.body.id_cliente, req.body.fecha_inicio, req.body.fecha_final, req.body.bloque, req.body.posicion_bloque, req.body.ultima_donacion, req.body.vueltas, req.body.donacion_activa, req.params.id];
            con.query("update subscripciones set id_detalles_sub = ?, id_cliente = ?, fecha_inicio = ?, fecha_final = ?, bloque = ?, posicion_bloque = ?, ultima_donacion = ?, vueltas = ?, donacion_activa = ? where id_subscripcion = ?", param, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });
*/
        }
    });
});
app.put('/tzedakin/api/unsubscribir/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("UPDATE subscripciones SET posicion_bloque = CASE WHEN posicion_bloque < ? THEN posicion_bloque ELSE posicion_bloque - 1 END,bloque = CASE WHEN posicion_bloque IN (1,2) THEN 1 WHEN posicion_bloque IN (3,4) THEN 2  WHEN posicion_bloque IN (5,6) THEN 3 WHEN posicion_bloque IN (7,8) THEN 4 WHEN posicion_bloque IN (9,10) THEN 5 WHEN posicion_bloque IN (11,12) THEN 6 END ORDER BY bloque ASC, posicion_bloque ASC", req.body.posicion, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });
        }
    });
})
app.delete('/tzedakin/api/subscripciones/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("delete from subscripciones where id_cliente = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});


// ------------------------------------------ categorias producto -----------------------//
app.get('/tzedakin/api/categorias/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from categorias where id_categoria = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });
        }
    });
});
app.get('/tzedakin/api/categorias/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from categorias", function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.post('/tzedakin/api/categorias/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = req.body.categoria;
            con.query("insert into categorias (categoria) values (?)", param, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.put('/tzedakin/api/categorias/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.categoria, req.params.id];
            con.query("update categorias set categoria = ? where id_categoria = ?", param, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });
        }
    });
});
app.delete('/tzedakin/api/categorias/:id', (req, res) => {
    const con = conectar();

    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("delete from categorias where id_categoria = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
// --------------------------------------- productos -------------------------- //
app.get('/tzedakin/api/productos/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select p.*,t.tipo_envio,ci.id_pais from productos p"
            +" inner join tienda t on  p.id_cliente=t.id_cliente"
            +" inner join ciudades ci on ci.id_ciudad = t.id_ciudad "
            , function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});

app.get('/tzedakin/api/productos_inner/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select pr.*, cl.id_cliente, cl.nombres, cl.apellidos, cl.correo, cat.categoria, es.estado from productos pr left join clientes cl on pr.id_cliente = cl.id_cliente left join categorias cat on pr.id_categoria = cat.id_categoria left join estados es on pr.id_estado = es.id_estado", function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});

app.get('/tzedakin/api/productos_inner/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select pr.*,  cat.categoria, es.estado from productos pr left join clientes cl on pr.id_cliente = cl.id_cliente left join categorias cat on pr.id_categoria = cat.id_categoria left join estados es on pr.id_estado = es.id_estado where cl.id_cliente = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});

app.get('/tzedakin/api/productos/:id', (req, res) => {

    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from productos where id_producto = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.post('/tzedakin/api/productos/', (req, res) => { 
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.nombre, req.body.descripcion, req.body.precio, req.body.id_categoria, req.body.id_estado, req.body.img_blob, req.body.id_cliente,req.body.peso,req.body.stock,req.body.alto,req.body.ancho,req.body.profundo];
            con.query("insert into productos (nombre, descripcion,  precio, id_categoria, id_estado, img_blob, id_cliente,peso,stock,alto,ancho,profundo) values (?)", [param], function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.put('/tzedakin/api/productos/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.nombre, req.body.descripcion, req.body.precio, req.body.id_estado, req.body.img_blob, req.body.id_cliente,req.body.peso,req.body.stock,req.body.alto,req.body.ancho,req.body.profundo,req.body.id_producto];
            con.query("update productos set nombre = ?, descripcion = ?, precio = ?,  id_estado = ?, img_blob = ?, id_cliente= ?,peso = ?,stock = ?,alto = ?,ancho = ?,profundo = ? where id_producto = ?", param, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.put('/tzedakin/api/productos_stock/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.stock,req.body.id_producto];
            con.query("update productos set stock = ? where id_producto = ?", param, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.put('/tzedakin/api/producto_autorizacion/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.id_estado, req.params.id];
            con.query("update productos set id_estado = ? where id_producto = ?", param, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
// ruta envio email de aceptacion
app.post('/tzedakin/api/correo_aceptar_producto', (req, res) => {
            res.status(200).send('Correo electrónico enviado correctamente');
});

app.delete('/tzedakin/api/productos/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("delete from productos where id_producto = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.get('/tzedakin/api/productos_innercategorias/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from productos pro left join categorias cat on pro.id_categoria = cat.id_categoria where id_producto = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    }); 0
});
app.get('/tzedakin/api/productos_innercategorias/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from productos pro left join categorias cat on pro.id_categoria = cat.id_categoria", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.get('/tzedakin/api/productos_innerestados/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from productos pro left join estados est on pro.id_estado = cat.id_estado where id_producto = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.get('/tzedakin/api/productos_innerestados/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from productos pro left join categorias cat on pro.id_categoria = cat.id_categoria", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
// -------------------------------------------  detalle_pedido -----------------------------
app.get('/tzedakin/api/detalle_pedido/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from detalle_pedido", function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.get('/tzedakin/api/detalle_pedido/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from detalle_pedido where id_detalle = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }

    });
});
app.post('/tzedakin/api/detalle_pedido', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.cantidad, req.body.precio_unidad, req.body.comentario, req.body.id_producto];
            con.query("insert into detalle_pedido (cantidad, precio_unidad, comentario, id_producto) values (?)", [param], function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.put('/tzedakin/api/detalle_pedido/:id', (req, res) => {
    const con = conectar();

    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.cantidad, req.body.precio_unidad, req.body.comentario, req.body.id_producto, req.params.id];
            con.query("update detalle_pedido set cantidad = ?, precio_unidad = ?, comentario = ?,  id_producto = ? where id_detalle = ?", param, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);

                }
                con.end();
            });

        }
    });
});
app.delete('/tzedakin/api/detalle_pedido/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("delete from detalle_pedido where id_detalle = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
// --------------------------------- solicitudes subscripciones ------------------------------------------
app.get('/tzedakin/api/solicitudes/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from solicitudes_subs", function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.get('/tzedakin/api/solicitudes_inner/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select subs.id_solicitud, cl.id_cliente, cl.nombres, cl.apellidos from solicitudes_subs subs join clientes cl on subs.id_cliente = cl.id_cliente", function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.get('/tzedakin/api/solicitudes/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select * from solicitudes_subs soli left join subscripciones sub on soli.id_cliente = sub.id_cliente where soli.id_cliente = ? or sub.id_cliente = ?", [req.params.id, req.params.id], function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    if (result)
                        res.send(result);
                }
                con.end();
            });

        }

    });
});
app.get('/tzedakin/api/solicitudes_cliente/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select activado from solicitudes_subs where id_cliente = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });
        }
    });
});
app.post('/tzedakin/api/solicitudes/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            comprobar_solicitud = "select soli.activado as activado from solicitudes_subs soli left join subscripciones sub on soli.id_cliente = sub.id_cliente where soli.id_cliente = ? or sub.id_cliente = ?;";
            con.query(comprobar_solicitud, [req.body.id_cliente, req.body.id_cliente], function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    if (result.length == 0) {
                        con.query("INSERT INTO solicitudes_subs (id_cliente, activado) VALUES (?, 0)", req.body.id_cliente, function (err, result) {
                            if (err) {
                                res.send(err);
                            } else {
                                res.send(JSON.stringify(result.affectedRows));
                                con.end();
                            }
                            con.end();
                        });
                    } else {
                        res.send(result);
                    }
                    con.end();
                }
            });
        }
    });
});
app.put('/tzedakin/api/solicitudes/:id', (req, res) => {
    const con = conectar();

    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.id_cliente, req.params.id];
            con.query("update solicitudes_subs set id_cliente = ?, activado = 1 where id_cliente = ?", param, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);

                }
                con.end();
            });

        }
    });
});
app.delete('/tzedakin/api/solicitudes/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("delete from solicitudes_subs where id_cliente = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
// --------------------------------- compras transacciones ------------------------------------------
app.post('/tzedakin/api/compras_transacciones/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.idComprador, req.body.idVendedor, req.body.cantidad, req.body.precio, req.body.fecha, req.body.idProducto,1];
            con.query("INSERT INTO compras_transacciones(id_comprador,id_vendedor,cantidad, precio,fecha,id_producto,id_estado)values (?)", [param], function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.get('/tzedakin/api/productos_comprados/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select pr.id_producto,pr.nombre as 'nombre_producto',ec.id as 'id_estado',ec.descripcion as 'estado',t.nombre as 'nombre_tienda'"
          +" ,t.direccion,c.ciudad,p.pais,pr.precio,pr.img_blob,cl.id_cliente,t.id as 'id_tienda',ct.fecha,ct.id as 'id_compra'"
          +" from compras_transacciones ct"
          +" inner join productos pr on ct.id_producto = pr.id_producto"
          +" inner join clientes cl on cl.id_cliente = ct.id_vendedor"
          +" inner join ciudades c on c.id_ciudad = cl.id_ciudad"
          +" inner join paises p on p.id_pais = c.id_pais"
          +" inner join estado_compra ec on ct.id_estado = ec.id"
          +" inner join tienda t on t.id_cliente = cl.id_cliente"
            +" where ct.id_comprador = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.get('/tzedakin/api/productos_vendidos/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select pr.id_producto,pr.nombre as 'nombre_producto',ec.id as 'id_estado',ec.descripcion as 'estado',cl.nombres as 'nombre_tienda'"
          +" ,cl.direccion,c.ciudad,p.pais,pr.precio,pr.img_blob,cl.id_cliente,ct.fecha, ct.id as 'id_compra'"
          +" from compras_transacciones ct"
          +" inner join productos pr on ct.id_producto = pr.id_producto"
          +" inner join clientes cl on cl.id_cliente = ct.id_comprador "
          +" inner join ciudades c on c.id_ciudad = cl.id_ciudad"
          +" inner join paises p on p.id_pais = c.id_pais"
          +" inner join estado_compra ec on ct.id_estado = ec.id"
            +" where ct.id_vendedor = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
// --------------------------------- comentarios ------------------------------------------
app.post('/tzedakin/api/comentarios_compra/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.idCliente, req.body.idProducto, req.body.idTienda, req.body.fecha, req.body.puntuacion, req.body.texto];
            con.query("INSERT INTO comentarios_compra(id_cliente,id_producto,id_tienda,fecha,puntuacion,texto)values (?)", [param], function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.get('/tzedakin/api/comentarios_producto/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select fecha,puntuacion,nombres as 'nombreCliente',texto"
          +" from comentarios_compra cc"
          +" inner join clientes cl on cl.id_cliente = cc.id_cliente"
            +" where cc.id_producto= ? order by fecha desc", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});app.get('/tzedakin/api/comentarios_tienda/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select fecha,cc.puntuacion,nombres as 'nombreCliente',texto"
          +" from comentarios_compra cc"
          +" inner join clientes cl on cl.id_cliente = cc.id_cliente"
          +" inner join tienda t on t.id = cc.id_tienda"
            +" where t.id_cliente= ? order by fecha desc", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});

//--------------------------chat----------------------
app.get('/tzedakin/api/mensajes/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select mc.* from mensaje_chat mc"
            +" inner join compras_transacciones ct on mc.id_compra = ct.id"
            +" where ct.id = ? order by fecha", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.post('/tzedakin/api/mensajes/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.id_compra, req.body.fecha, req.body.mensaje, req.body.id_usuario];
            con.query("INSERT INTO mensaje_chat(id_compra,fecha,mensaje,id_usuario)values (?)", [param], function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
//----------------------------------------------Tienda------------------------------------
app.get('/tzedakin/api/tienda/:id', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            con.query("select t.id,t.nombre,t.direccion,ci.ciudad,pa.pais,t.tipo_envio, t.id_cliente,t.puntuacion, t.correo, t.telefono,pa.id_pais,t.id_ciudad,t.subscrito, "
            +"t.color_fondo, t.color_letra_fondo, t.color_producto, t.color_letra_producto, t.color_comentario,t.color_letra_comentario from tienda t "
            +"inner join ciudades ci on t.id_ciudad = ci.id_ciudad "
            +"inner join paises pa on pa.id_pais = ci.id_pais "
            +"where t.id_cliente = ?", req.params.id, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.post('/tzedakin/api/tienda/', (req, res) => {
    const con = conectar();
    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.nombre, req.body.direccion, req.body.id_ciudad, req.body.tipo_envio, req.body.id_cliente, req.body.puntuacion, req.body.correo, req.body.telefono];
            con.query("INSERT INTO tienda(nombre,direccion,id_ciudad,tipo_envio,id_cliente,puntuacion,correo,telefono)values (?)", [param], function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);
                }
                con.end();
            });

        }
    });
});
app.put('/tzedakin/api/tienda/', (req, res) => {
    const con = conectar();

    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.nombre, req.body.direccion, req.body.id_ciudad, req.body.tipo_envio, req.body.id_cliente, req.body.puntuacion, req.body.correo, req.body.telefono,req.body.id];
            con.query("update tienda set nombre = ?, direccion = ?, id_ciudad = ?, tipo_envio = ?, id_cliente = ?, puntuacion = ?, correo = ?, telefono = ? where id = ?", param, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);

                }
                con.end();
            });

        }
    });
});
app.put('/tzedakin/api/tienda_diseno/', (req, res) => {
    const con = conectar();

    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.color_fondo, req.body.color_letra_fondo, req.body.color_producto, req.body.color_letra_producto, req.body.color_comentario, req.body.color_letra_comentario,req.body.id];
            con.query("update tienda set color_fondo = ?,color_letra_fondo = ?,color_producto = ?,color_letra_producto = ?,color_comentario = ?,color_letra_comentario = ? where id = ?", param, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);

                }
                con.end();
            });

        }
    });
});
app.put('/tzedakin/api/tienda_subscripcion/', (req, res) => {
    const con = conectar();

    con.connect((err) => {
        if (err) {
            res.send(err);
        } else {
            let param = [req.body.id];
            con.query("update tienda set subscrito = 1 where id = ?", param, function (err, result) {
                if (err) {
                    res.send(err);
                } else {
                    res.send(result);

                }
                con.end();
            });

        }
    });
});
app.set('port',8545);

app.listen(app.get('port'),()=>{
    console.log(`Server listening on port ${app.get('port')}`);
});