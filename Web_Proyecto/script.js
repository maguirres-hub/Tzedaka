// ----------------------- Paises --------------------------//
function Validar_Paises() {
    let form = document.forms['formulario_paises'];

    let json = {
        "pais": form[1].value,
    }
    Insert_Pais(json);
    return false;
}



function Insert_Pais(json) {
    var registro = confirm("¿Desea registrar un pais con el nombre de " + json.pais + "?", "Si", "Cancelar");
    if (registro == true) {
        var url = 'https://berajotweb.com/tzedakin/api/paises/';
        fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(json)
        })
            .then(response => response.json())
            .then(datos => {
                if (datos.errno !== 1062) {
                    alert('registro exitoso');
                    Limpiar_Tabla("pais");
                    Get_Paises();
                    Limpiar_Formulario_Pais();
                } else {
                    alert('Error: No se pudo registrar el pais.!');
                }
            })
            .catch(error => {
                console.error("Error encontrado: " + error);
            });
    }
}




function Get_Paises() {
    var loading = document.getElementById('loading_pais');
    loading.style.display = 'block';
    var url = 'https://berajotweb.com/tzedakin/api/paises/';
    fetch(url, {
        method: 'GET',
    })
        .then(response => response.json())
        .then(datos => {
            if (datos.length > 0) {
                for (let x = 0; x < datos.length; x++) {
                    Limpiar_Tabla("pais");
                    setTimeout(Tabla_Paises, 100 * x, datos[x], x);

                }
            } else {
                alert("No has registrado paises.!");
            }
            loading.style.display = 'none';

        })
        .catch(error => {
            console.error("Error encontrado: ", error);
            loading.style.display = 'none';
        });
}

function Tabla_Paises(datos, cantidad) {
    let json = {
        "id_pais": datos.id_pais,
        "pais": datos.pais
    };
    arreglo = [cantidad + 1, json.pais, "<a class='btn btn-success' onclick='Modificar_Pais(" + JSON.stringify(datos) + ")'>Modificar</a>", "<a class='btn btn-danger' onclick='Eliminar_Pais(" + JSON.stringify(datos) + ")'>Eliminar</a>"];
    let tr = document.createElement('tr');
    for (let x = 0; x < arreglo.length; x++) {
        let td = document.createElement('td');
        td.innerHTML = arreglo[x];
        tr.appendChild(td);
    }
    document.getElementById('tabla_paises').appendChild(tr);
}

function Modificar_Pais(json) {
    const form = document.forms['formulario_paises'];
    document.getElementById("btn_update_pais").classList.remove("disabled");
    document.getElementById('btn_pais').setAttribute("disabled", true);
    form[0].value = json.id_pais;
    form[1].value = json.pais;
    document.documentElement.scrollTop = 0;
}
function Put_Pais() {
    var confirmar = confirm("¿Desea actualizar con estos datos?", "Si", "No");
    if (confirmar == true) {
        let form = document.forms['formulario_paises'];
        let json = {
            "pais": form[1].value,
        }
        var url = 'https://berajotweb.com/tzedakin/api/paises/' + form[0].value;
        fetch(url, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(json)
        })
            .then(response => response.json())
            .then(datos => {
                if (datos.affectedRows > 0) {
                    alert('Datos Actualizados');
                    Limpiar_Tabla('pais');
                    Get_Paises();
                    Limpiar_Formulario_Pais();
                } else {
                    alert('Ocurrio un problema al actualizar.!');
                }

            })
            .catch(error => console.error("Error encontrado: ", error));
    }
}

function Eliminar_Pais(json) {
    var confirmar = confirm("¿Desea eliminar este pais?", "Si", "No");
    if (confirmar == true) {
        let form = document.forms['formulario_paises'];
        form[0].value = json.id_pais;
        var url = 'https://berajotweb.com/tzedakin/api/paises/' + form[0].value;
        console.log(url);
        fetch(url, {
            method: 'DELETE',
        })
            .then(response => response.json())
            .then(datos => {
                if (datos.affectedRows > 0) {
                    alert('Datos Eliminados');
                    Limpiar_Tabla('pais');
                    Get_Paises();
                    Limpiar_Formulario_Pais();
                }
                else {
                    alert('Ocurrio un error durante la eliminación.!');
                }
            })
            .catch(error => console.error("Error encontrado: ", error));
    }

}
function Limpiar_Select_Paises() {
    var paises = document.getElementById('select_paises');
    var ultima_pais = paises.lastElementChild;
    while (ultima_pais) {
        paises.removeChild(ultima_pais);
        ultima_pais = paises.lastElementChild;
    }
    var opciones = document.createElement('option');
    opciones.innerHTML = "Seleccione el pais";
    opciones.value = "";
    paises.appendChild(opciones);
}
function Limpiar_Formulario_Pais() {
    let form = document.forms['formulario_paises'];
    for (let i = 0; i < form.length; i++) {
        form[i].value = "";
    }
    document.getElementById("btn_update_pais").classList.add("disabled");
    document.getElementById('btn_pais').removeAttribute('disabled');
}
// ------------------------------- Ciudades -------------------------//
function Validar_Ciudades() {
    let form = document.forms['formulario_ciudades'];
    let json = {
        "id_pais": form[1].value,
        "ciudad": form[2].value,
    }
    Insert_Ciudad(json);
    return false;
}



function Insert_Ciudad(json) {
    var registro = confirm("¿Desea registrar un pais con el nombre de " + json.ciudad + " ?", "Si", "Cancelar");
    if (registro == true) {
        var url = 'https://berajotweb.com/tzedakin/api/ciudades/';
        fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(json)
        })
            .then(response => response.json())
            .then(datos => {
                if (datos.errno !== 1062) {
                    alert('registro exitoso');
                    Limpiar_Tabla("ciudad");
                    Get_Ciudades();
                } else {
                    alert('Error: No se pudo registrar el pais.!');
                }
            })
            .catch(error => {
                console.error("Error encontrado: " + error);
            });
    }
}
function Get_Ciudades() {
    var loading = document.getElementById('loading_ciudad');
    loading.style.display = 'block';
    var url = 'https://berajotweb.com/tzedakin/api/ciudades_inner_pais/';
    fetch(url, {
        method: 'GET',
    })
        .then(response => response.json())
        .then(datos => {
            if (datos.length > 0) {
                for (let x = 0; x < datos.length; x++) {
                    setTimeout(Tabla_Ciudades, 100 * x, datos[x], x);
                    Limpiar_Tabla('ciudad');
                }
            } else {
                alert("No tienes ciudades registradas.!");
            }
            Get_Paises_Select();
            loading.style.display = 'none';
        })
        .catch(error => {
            console.error("Error encontrado: ", error);
            loading.style.display = 'none';
        });
}
function Get_Paises_Select() {
    var loading = document.getElementById('loading_ciudad');
    loading.style.display = 'block';
    var url = 'https://berajotweb.com/tzedakin/api/paises/';
    fetch(url, {
        method: 'GET',
    })
        .then(response => response.json())
        .then(datos => {
            Limpiar_Select_Paises();
            if (datos.length > 0) {
                for (let x = 0; x < datos.length; x++) {
                    setTimeout(Select_Paises, 100 * x, datos[x]);
                }
            } else {
                alert("No has registrado paises.!");
            }
            loading.style.display = 'none';

        })
        .catch(error => {
            console.error("Error encontrado: ", error);
            loading.style.display = 'none';
        });
}
function Select_Paises(json) {
    var loading = document.getElementById('loading_ciudad');
    loading.style.display = 'block';
    
    var json2 = {
        "id": json.id_pais,
        "pais": json.pais
    }
    arreglo  = [json2.id, json2.pais];
    var opciones = document.createElement('option');
    for(let i = 0; i <= arreglo.length; i++){
        if(i == 0){
            opciones.value = arreglo[0];
        }else{
            opciones.innerHTML = arreglo[1];
        }
   }
    document.getElementById('select_paises').appendChild(opciones);
    loading.style.display = 'none';
}
function Tabla_Ciudades(datos, cantidad) {
    let json = {
        "id_ciudad": datos.id_ciudad,
        "ciudad": datos.ciudad,
        "id_pais": datos.id_pais,
        "pais": datos.pais,
    };
    arreglo = [cantidad + 1, json.ciudad, json.pais, "<a class='btn btn-success' onclick='Modificar_Ciudad(" + JSON.stringify(datos) + ")'>Modificar</a>", "<a class='btn btn-danger' onclick='Eliminar_Ciudad(" + JSON.stringify(datos) + ")'>Eliminar</a>"];
    let tr = document.createElement('tr');
    for (let x = 0; x < arreglo.length; x++) {
        let td = document.createElement('td');
        td.innerHTML = arreglo[x];
        tr.appendChild(td);
    }
    document.getElementById('tabla_ciudades').appendChild(tr);
}
function Modificar_Ciudad(json) {
    const form = document.forms['formulario_ciudades'];
    document.getElementById('btn_ciudad').setAttribute("disabled", true);
    document.getElementById("btn_update_ciudad").classList.remove("disabled");
    form[0].value = json.id_ciudad;
    form[1].value = json.id_pais;
    form[2].value = json.ciudad;
    document.documentElement.scrollTop = 0;
}
function Put_Ciudad() {
    var confirmar = confirm("¿Desea actualizar con estos datos?", "Si", "No");
    if (confirmar == true) {
        let form = document.forms['formulario_ciudades'];
        let json = {
            "id_pais": form[1].value,
            "ciudad": form[2].value,
        }
        var url = 'https://berajotweb.com/tzedakin/api/ciudades/' + form[0].value;
        console.log(url);
        fetch(url, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(json)
        })
            .then(response => response.json())
            .then(datos => {
                if (datos.affectedRows > 0) {
                    alert('Datos Actualizados');
                    Limpiar_Tabla('ciudad');
                    Get_Ciudades();
                    Limpiar_Formulario_Ciudad();
                } else {
                    alert('Ocurrio un problema al actualizar.!');
                }

            })
            .catch(error => console.error("Error encontrado: ", error));
    }
}

function Eliminar_Ciudad(json) {
    var confirmar = confirm("¿Desea eliminar esta ciudad?", "Si", "No");
    if (confirmar == true) {
        let form = document.forms['formulario_ciudades'];
        form[0].value = json.id_ciudad;
        var url = 'https://berajotweb.com/tzedakin/api/ciudades/' + form[0].value;
        fetch(url, {
            method: 'DELETE',
        })
            .then(response => response.json())
            .then(datos => {
                if (datos.affectedRows > 0) {
                    alert('Datos Eliminados');
                    Limpiar_Tabla('ciudad');
                    Get_Ciudades();
                    Limpiar_Formulario_Ciudad();
                }
                else {
                    alert('Ocurrio un error durante la eliminación.!');
                }
            })
            .catch(error => console.error("Error encontrado: ", error));
    }

}



function Limpiar_Select_Ciudades() {
    var ciudades = document.getElementById('select_ciudades');

    var ultima_ciudad = ciudades.lastElementChild;
    while (ultima_ciudad) {
        ciudades.removeChild(ultima_ciudad);
        ultima_ciudad = ciudades.lastElementChild;
    }
    var opciones = document.createElement('option');
    opciones.innerHTML = "Seleccione la ciudad";
    opciones.value = "";
    ciudades.appendChild(opciones);
}
function Limpiar_Formulario_Ciudad() {
    let form = document.forms['formulario_ciudades'];
    for (let i = 0; i < form.length; i++) {
        form[i].value = "";
    }
    document.getElementById("btn_update_ciudad").classList.add("disabled");
    document.getElementById('btn_ciudad').removeAttribute('disabled');
}

// ------------------------------ Empleados ------------------//
function Validar_Empleado() {
    let form = document.forms['formulario_empleados'];
    var date = new Date();
    var year = date.getFullYear();
    var mes = date.getMonth();
    var dia = date.getDate();
    if (mes.toString().length < 2) {
        mes = '0' + mes;
    }
    fecha = year + "-" + mes + "-" + dia;
    let json = {
        "nombres": form[1].value,
        "apellidos": form[2].value,
        "correo": form[3].value,
        "fecha_registroem": fecha,
        "password": form[4].value,
        "active": 1
    }
    Insert_Empleado(json);
    return false;
}



function Insert_Empleado(json) {
    var url = 'https://berajotweb.com/tzedakin/api/empleados/';
    fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(json)
    })
        .then(response => response.json())
        .then(datos => {
            if (datos.errno !== 1062) {
                alert('registro exitoso');
                Limpiar_Tabla("empleados");
                Limpiar_Formulario_Empleado();
                Get_Empleados();
            } else {
                alert('Error: Ya existe registrado un usuario con este correo electronico.!');
            }
        })
        .catch(error => {
            console.error("Error encontrado: " + error);
        });
}
function Get_Empleados() {
    var loading = document.getElementById('loading_empleados');
    loading.style.display = 'block';
    var url = 'https://berajotweb.com/tzedakin/api/empleados/';
    fetch(url, {
        method: 'GET'
    })
        .then(response => response.json())
        .then(datos => {
            Limpiar_Tabla("empleados");
            for (let i = 0; i < datos.length; i++) {

                setTimeout(Tabla_Empleados, 100 * 1, datos[i], i);
            }
            loading.style.display = 'none';
        })
        .catch(error => console.error("Error encontrado: ", error));
}
function Put_Empleado() {
    let form = document.forms['formulario_empleados'];
    if (form[0].value == "") { alert("Click en el boton modificar de los datos en la tabla que desea modificar!"); return false };
    let json = {
        "nombres": form[1].value,
        "apellidos": form[2].value,
        "correo": form[3].value,
        "password": form[4].value
    };
    var url = 'https://berajotweb.com/tzedakin/api/empleados/' + form[0].value;
    fetch(url, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(json)
    })
        .then(response => response.json())
        .then(datos => {
            alert('Datos Actualizados');
            Limpiar_Tabla('empleados');
            Limpiar_Formulario_Empleado();
            Get_Empleados();
        })
        .catch(error => console.error("Error encontrado: ", error));
}

function Tabla_Empleados(datos, cantidad) {

    let arreglo = [cantidad + 1, datos.nombres, datos.apellidos, datos.correo, "<a class='btn btn-success' onclick='Modificar_Empleado(" + JSON.stringify(datos) + ")'>Modificar</a>", "<a class='btn btn-danger' onclick='Eliminar_Empleado(" + JSON.stringify(datos) + ")'>Eliminar</a>"];
    let tr = document.createElement('tr');
    for (let x = 0; x < arreglo.length; x++) {
        let td = document.createElement('td');
        td.innerHTML = arreglo[x];
        tr.appendChild(td);
    }
    document.getElementById('tabla_empleados').appendChild(tr);

}
function Modificar_Empleado(json) {
    let form = document.forms['formulario_empleados'];
    let btn_empleado = document.getElementById('btn_empleado');
    btn_empleado.setAttribute("disabled", true);
    form[0].value = json.id_empleado;
    form[1].value = json.nombres;
    form[2].value = json.apellidos;
    form[3].value = json.correo;
    form[4].value = json.password;
    document.documentElement.scrollTop = 0;
}
function Eliminar_Empleado(json) {

    let form = document.forms['formulario_empleados'];
    form[0].value = json.id_empleado;
    var url = 'https://berajotweb.com/tzedakin/api/empleados/' + form[0].value;
    fetch(url, {
        method: 'DELETE',
    })
        .then(response => response.json())
        .then(datos => {
            alert('Datos Eliminados');
            Limpiar_Tabla('empleados');
            Limpiar_Formulario_Empleado();
            Get_Empleados();
        })
        .catch(error => console.error("Error encontrado: ", error));
}
function Limpiar_Formulario_Empleado() {
    let form = document.forms['formulario_empleados'];
    for (let i = 0; i < form.length; i++) {
        form[i].value = "";
    }
    document.getElementById('btn_empleado').removeAttribute('disabled');
}
// ---------------------------------------- clientes -------------------------
function Validar_Cliente() {
    let form = document.forms['formulario_clientes'];
    var date = new Date();
    var year = date.getFullYear();
    var mes = date.getMonth();
    var dia = date.getDate();
    if (mes.toString().length < 2) {
        mes = '0' + mes;
    }
    fecha = year + "-" + mes + "-" + dia
    let json = {
        "nombres": form[1].value,
        "apellidos": form[2].value,
        "telefono": "+57" + form[3].value,
        "direccion": form[4].value,
        "correo": form[5].value,
        "password": form[6].value,
        //"pais": form[6].value,
        "id_ciudad": form[8].value,
        "fecha_registrocl": fecha,
        "active": 1
    }
    Insert_Cliente(json);
    return false;
}
function Insert_Cliente(json) {
    var url = 'https://berajotweb.com/tzedakin/api/clientes/';
    fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(json)
    })
        .then(response => response.json())
        .then(datos => {
            if (datos.errno !== 1062) {
                Insert_Billetera(datos.insertId);
            } else {
                alert('Error: Ya existe registrado un usuario con este correo electronico.!');
            }
        })
        .catch(error => {
            console.error("Error encontrado: " + error);
        });
}
function Get_Clientes() {
    //var loading = document.getElementById('loading_clientes');
    //loading.style.display = 'block';
    var url = 'https://berajotweb.com/tzedakin/api/clientes_inner/';
    fetch(url, {
        method: 'GET'
    })
        .then(response => response.json())
        .then(datos => {
            Limpiar_Tabla("clientes");
            for (let i = 0; i < datos.length; i++) {
                setTimeout(Tabla_Clientes, 500 * 1, datos[i], i);
            }
         //   loading.style.display = 'none';
        })
        .catch(error => console.error("Error encontrado: ", error));
}
function Tabla_Clientes(datos, cantidad) {
    console.log(datos)
    let date = new Date(datos.fecha_registrocl);
    let year = date.getFullYear();
    let mes = date.getMonth();
    let dia = date.getDate();
    let fecha = `${year}-${mes}-${dia}`;
    let arreglo;
    let subs;
    let activado;
    let btn_bloq;
    (datos.id_subscripcion == null) ? subs = "Desactivado" : subs = "Activado";
    (datos.active == 0) ? activado = "Bloqueado" : activado = "Desbloqueado";
    (datos.active == 0) ? btn_bloq = "<a class='btn btn-secondary text-white' onclick='Desbloquear_Cliente(" + JSON.stringify(datos.id_cliente) + ")'>Desbloquear</a>" : btn_bloq = "<a class='btn btn-warning text-white' onclick='Bloquear_Cliente(" + JSON.stringify(datos.id_cliente) + ")'>Bloquear</a>";
    arreglo = [cantidad + 1, datos.nombres, datos.apellidos, datos.telefono, datos.direccion, datos.correo, /*datos.pais,*/ datos.ciudad, subs, fecha, activado, "<a class='btn btn-success' onclick='Modificar_Cliente(" + JSON.stringify(datos) + ")'>Modificar</a>", "<a class='btn btn-info text-white' onclick='Get_Posicion_Bloque(" + JSON.stringify(datos.id_cliente) + ")'>Subscribir</a>", btn_bloq, "<a class='btn btn-danger' onclick='Eliminar_Cliente(" + JSON.stringify(datos) + ")'>Eliminar</a>"];

    let tr = document.createElement('tr');
    for (let x = 0; x < arreglo.length; x++) {
        let td = document.createElement('td');
        td.innerHTML = arreglo[x];
        tr.appendChild(td);
    }
    document.getElementById('tabla_clientes').appendChild(tr);

}
function Modificar_Cliente(json) {
    let form = document.forms['formulario_clientes'];
    document.getElementById('btn_cliente').setAttribute('disabled', true);
    form[0].value = json.id_cliente;
    form[1].value = json.nombres;
    form[2].value = json.apellidos;
    form[3].value = json.telefono;
    form[4].value = json.direccion;
    form[5].value = json.correo;
    form[6].value = json.password;
    form[8].value = json.id_ciudad;

    document.documentElement.scrollTop = 0;
}
function Put_Cliente() {
    let form = document.forms['formulario_clientes'];
    let json = {
        "nombres": form[1].value,
        "apellidos": form[2].value,
        "telefono": form[3].value,
        "direccion": form[4].value,
        "correo": form[5].value,
        "password": form[6].value,
        "id_ciudad": form[8].value
    };
    var url = 'https://berajotweb.com/tzedakin/api/clientes/' + form[0].value;
    fetch(url, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(json)
    })
        .then(response => response.json())
        .then(datos => {
            alert('Datos Actualizados');
            Limpiar_Tabla('clientes');
            Limpiar_Formulario_Clientes();
            Get_Clientes();
        })
        .catch(error => console.error("Error encontrado: ", error));
}
function Bloquear_Cliente(id_cliente) {
    let json = {
        "active": 0
    }
    var url = 'https://berajotweb.com/tzedakin/api/clientes_estatus/' + id_cliente;
    fetch(url, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(json)
    })
        .then(response => response.json())
        .then(datos => {
            alert('Datos Actualizados');
            Limpiar_Tabla('clientes');
            Limpiar_Formulario_Clientes();
            Get_Clientes();
        })
        .catch(error => console.error("Error encontrado: ", error));
}
function Desbloquear_Cliente(id_cliente) {
    let json = {
        "active": 1
    }
    var url = 'https://berajotweb.com/tzedakin/api/clientes_estatus/' + id_cliente;
    fetch(url, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(json)
    })
        .then(response => response.json())
        .then(datos => {
            alert('Datos Actualizados');
            Limpiar_Tabla('clientes');
            Limpiar_Formulario_Clientes();
            Get_Clientes();
        })
        .catch(error => console.error("Error encontrado: ", error));
}
function Eliminar_Cliente(json) {
    let form = document.forms['formulario_clientes'];
    form[0].value = json.id_cliente;
    var url = 'https://berajotweb.com/tzedakin/api/clientes/' + form[0].value;
    fetch(url, {
        method: 'DELETE',
    })
        .then(response => response.json())
        .then(datos => {
            alert('Datos Eliminados');
            Limpiar_Tabla('clientes');
            Get_Clientes();
        })
        .catch(error => console.error("Error encontrado: ", error));
}

function Limpiar_Formulario_Clientes() {
    let form = document.forms['formulario_clientes'];
    let select = document.getElementById('select_ciudades');
    select.value = "default";
    for (let i = 0; i < form.length; i++) {
        form[i].value = "";
    }
    document.getElementById('btn_cliente').removeAttribute('disabled');
}
// ----------------------------------------- Billetera Virtual --------------------------------------//

function Insert_Billetera(id_cliente) {
    json = {
        "id_cliente": id_cliente,
        "total": 0
    };
    var url = 'https://berajotweb.com/tzedakin/api/billetera_virtual/';
    fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(json)
    })
        .then(response => response.json())
        .then(datos => {
            if (datos.affectedRows === 1) {
                Limpiar_Tabla("clientes");
                Limpiar_Formulario_Clientes();
                Get_Clientes();
                alert('registro exitoso.!');
            }
        })
        .catch(error => {
            console.error("Error encontrado: " + error);
        });
}
// ----------------------------------------- detalles subscripcion ------------------------------//
function Get_Detalles_Subscripcion() {
    var url = 'https://berajotweb.com/tzedakin/api/detalles_subscripcion/';
    fetch(url, {
        method: 'GET'
    })
        .then(response => response.json())
        .then(datos => {
            if (datos.length > 0) {
                Limpiar_Formulario_Detalles_Sub();
                Limpiar_Tabla('tabla_subscriptores');
                let form = document.forms['formulario_detalles_sub'];
                form[0].value = datos[0].id_detalles_sub;
                form[1].value = datos[0].precio;
            }

        })
        .catch(error => console.error("Error encontrado: ", error));
}
///tzedakin/api/subscripciones

function Get_Subscriptores() {
    var url = 'https://berajotweb.com/tzedakin/api/subscripciones_innner/';
    fetch(url, {
        method: 'GET'
    })
        .then(response => response.json())
        .then(datos => {
            if (datos.length > 0) {
                for (let i = 0; i < datos.length; i++) {
                    Limpiar_Tabla('subscriptores');
                    setTimeout(Tabla_Subscriptores, i * 100, datos[i], i + 1);
                }
            }
        })
        .catch(error => console.error("Error encontrado: ", error));
}
function Tabla_Subscriptores(subs, cantidad) {
    const arreglo = [cantidad + 1, subs.nombres, subs.apellidos, subs.telefono, subs.correo, subs.ciudad, subs.bloque, subs.posicion_bloque, "<a class='btn btn-danger' onclick='Actualizar_Posicion_Bloque(" + JSON.stringify(subs.id_subscripcion) + ", " + JSON.stringify(subs.posicion_bloque) + ")'>Cancelar Subscripcion</a>"];
    let tr = document.createElement('tr');
    for (let x = 0; x < arreglo.length; x++) {
        let td = document.createElement('td');
        td.innerHTML = arreglo[x];
        tr.appendChild(td);
    }
    document.getElementById('tabla_subscriptores').appendChild(tr);

}
function Put_Detalles_Sub() {
    var form = document.forms['formulario_detalles_sub'];
    let json = {
        "precio": form[2].value
    }
    var url = 'https://berajotweb.com/tzedakin/api/detalles_subscripcion/' + form[0].value;
    fetch(url, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(json)
    })
        .then(response => response.json())
        .then(datos => {
            alert('Datos Actualizados');
            Limpiar_Formulario_Detalles_Sub();
            Get_Detalles_Subscripcion();
        })
        .catch(error => console.error("Error encontrado: ", error));
}

function Get_Posicion_Bloque(id_cliente) {
    var url = 'https://berajotweb.com/tzedakin/api/subscripciones';
    fetch(url, {
        method: 'GET'
    })
        .then(response => response.json())
        .then(datos => {
            if (datos.length > 0) {
                for (let i = 0; i < datos.length; i++) {
                    var ultima_posicion = datos[i].posicion_bloque;
                }
                nuevaposicion = parseInt(ultima_posicion) + parseInt(1);
                if (ultima_posicion == 12) {
                    alert("Ya no puede subscribir a nadie mas ya estan lleno los 6 bloques.!")
                } else {
                    if (ultima_posicion == 1) {
                        Subscribir_Cliente(id_cliente, 1, nuevaposicion);
                    }
                    else if (ultima_posicion == 2 || ultima_posicion == 3) {
                        Subscribir_Cliente(id_cliente, 2, nuevaposicion);
                    }
                    else if (ultima_posicion == 4 || ultima_posicion == 5) {
                        Subscribir_Cliente(id_cliente, 3, nuevaposicion);
                    }
                    else if (ultima_posicion == 6 || ultima_posicion == 7) {
                        Subscribir_Cliente(id_cliente, 4, nuevaposicion);
                    }
                    else if (ultima_posicion == 8 || ultima_posicion == 9) {
                        Subscribir_Cliente(id_cliente, 5, nuevaposicion);
                    }
                    else if (ultima_posicion == 10 || ultima_posicion == 11) {
                        Subscribir_Cliente(id_cliente, 6, nuevaposicion);
                    }
                }
            }
            else if (datos.length == 0) {
                Subscribir_Cliente(id_cliente, 1, 1);
            }
        })
        .catch(error => console.error("Error encontrado: ", error));
}
function Subscribir_Cliente(id_cliente, bloque, posicion) {
    var url = 'https://berajotweb.com/tzedakin/api/subscripciones/';
    let date = new Date();
    let year = date.getFullYear();
    let mes = date.getMonth() + 1;
    let dia = date.getDate();
    if (mes.toString().length < 2) {
        mes = '0' + mes;
    }
    let ultimodiames = new Date(date.getFullYear(), date.getMonth() + 1, 0).getDate();
    let fecha_subscripcion = `${year}` + "-" + `${mes}` + "-" + `${dia}`;
    (mes == 12) ? year = year + 1 : year = year;
    if (ultimodiames === 28) {
        dia = parseInt(28) - parseInt(dia);
        mes = parseInt(mes) + parseInt(1)
        if (mes.toString().length < 2) {
            mes = '0' + mes;
        }
        dia = parseInt(28) - parseInt(dia);
    } else if (ultimodiames === 29) {
        dia = parseInt(29) - parseInt(dia);
        mes = parseInt(mes) + parseInt(1);
        if (mes.toString().length < 2) {
            mes = '0' + mes;
        }
        dia = parseInt(29) - parseInt(dia);
    } if (ultimodiames === 30) {
        dia = parseInt(30) - parseInt(dia);
        mes = parseInt(mes) + parseInt(1);
        if (mes.toString().length < 2) {
            mes = '0' + mes;
        }
        dia = parseInt(30) - parseInt(dia);
    } else if (ultimodiames === 31) {
        dia = parseInt(31) - parseInt(dia);
        mes = parseInt(mes) + parseInt(1);
        if (mes.toString().length < 2) {
            mes = '0' + mes;
        }
        dia = parseInt(31) - parseInt(dia);
    }
    let fecha_cancelacion = `${year}` + "-" + `${mes}` + "-" + `${dia}`;
    json = {
        'id_cliente': id_cliente,
        'fecha_inicio': fecha_subscripcion,
        'fecha_final': fecha_cancelacion,
        'bloque': bloque,
        'posicion_bloque': posicion,
        'ultima_donacion': 0,
    };
    fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(json)
    })
        .then(response => response.json())
        .then(datos => {
            if (datos.affectedRows > 0) {
                alert('registro de subscripcion exitosa.!');
                Limpiar_Tabla("clientes");
                Limpiar_Formulario_Clientes();
                Get_Clientes();
            }
        })
        .catch(error => {
            console.error("Error encontrado: " + error);
        });
}
function Actualizar_Posicion_Bloque(id_subscripcion, posicion_bloque) {
    var url = 'https://berajotweb.com/tzedakin/api/unsubscribir/';
    let json = {
        "posicion": posicion_bloque
    }
    fetch(url, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(json)

    })
        .then(response => response.json())
        .then(datos => {
            if (datos.affectedRows > 0) {
                Eliminar_Subscripcion(id_subscripcion);
            }
        })
        .catch(error => console.error("Error encontrado: ", error));
}
function Eliminar_Subscripcion(id_subscripcion) {
    var url = 'https://berajotweb.com/tzedakin/api/subscripciones/' + id_subscripcion;
    fetch(url, {
        method: 'DELETE',
    })
        .then(response => response.json())
        .then(datos => {
            if (datos.affectedRows === 1) {
                alert('Subscripcion Eliminada');
                Limpiar_Tabla("subscriptores");
                //Limpiar_Formulario_Detalles_Sub();
                Get_Subscriptores();
            }
        })
        .catch(error => console.error("Error encontrado: ", error));
}
function Limpiar_Formulario_Detalles_Sub() {
    var form = document.forms['formulario_detalles_sub'];
    form[2].value = "";
}
// ---------------------------------------- pagos -------------------------
function validar_pago() {
    let form = document.forms['formulario_pagos'];
    let json = {
        "forma_pago": form[1].value,
        "fecha_pago": form[2].value,
        "id_cliente": form[3].value
    }
    Insert_Pago(json);
    return false;
}
function Insert_Pago(json) {
    var url = 'https://berajotweb.com/tzedakin/api/pago/';
    fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(json)
    })
        .then(response => response.json())
        .then(datos => {
            alert('registro exitoso');
            Limpiar_Tabla("pagos");
            Get_Pagos();
        })
        .catch(error => console.error("Error encontrado: ", error));
}
function Get_Pagos() {
    var url = 'https://berajotweb.com/tzedakin/api/pago/';
    fetch(url, {
        method: 'GET'
    })
        .then(response => response.json())
        .then(datos => {
            Limpiar_Tabla("pagos");
            for (let i = 0; i < datos.length; i++) {
                Eliminar_Select_Pago();
                setTimeout(Tabla_Pagos, 100 * 1, datos[i], i);

            }
            Get_Select_Pago();
        })
        .catch(error => console.error("Error encontrado: ", error));
}
function Get_Select_Pago() {
    var url = 'https://berajotweb.com/tzedakin/api/pago';
    fetch(url, {
        method: 'GET'
    })
        .then(response => response.json())
        .then(datos => {
            for (let i = 0; i < datos.length; i++) {
                Eliminar_Select_Pago();
                setTimeout(Seleccion_Pago, 100 * 1, datos[i], i);
            }
        })
        .catch(error => console.error("Error encontrado: ", error));
}

function Seleccion_Pago(datos) {
    let option = document.createElement('option');
    option.value = datos.id_pago;
    option.innerHTML = datos.forma_pago;
    document.getElementById('forma_pago').appendChild(option);
}
function Eliminar_Select_Pago() {
    let estado = document.getElementById('forma_pago');
    let ultimo = estado.lastElementChild;
    while (ultimo) {
        estado.removeChild(ultimo);
        ultimo = estado.lastElementChild;
    }
    let option = document.createElement('option');
    option.innerHTML = "Seleccione Estado";
    estado.appendChild(option);
}
function Put_Pago() {
    let form = document.forms['formulario_pagos'];
    let json = {
        "forma_pago": form[1].value,
        "fecha_pago": form[2].value,
        "id_cliente": form[3].value
    }
    var url = 'https://berajotweb.com/tzedakin/api/pago/' + form[0].value;
    fetch(url, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(json)
    })
        .then(response => response.json())
        .then(datos => {
            alert('Datos Actualizados');
            Limpiar_Tabla('pagos');
            Get_Pagos();
            Eliminar_Select_Pago();
            Get_Select_Pago();
        })
        .catch(error => console.error("Error encontrado: ", error));
}

function Tabla_Pagos(datos, cantidad) {

    let arreglo = [cantidad + 1, datos.forma_pago, datos.fecha_pago, datos.id_cliente, "<a class='btn btn-success' onclick='Modificar_Pago(" + JSON.stringify(datos) + ")'>Modificar</a>", "<a class='btn btn-danger' onclick='Eliminar_Pago(" + JSON.stringify(datos) + ")'>Eliminar</a>"];
    let tr = document.createElement('tr');
    for (let x = 0; x < arreglo.length; x++) {
        let td = document.createElement('td');
        td.innerHTML = arreglo[x];
        tr.appendChild(td);
    }
    document.getElementById('tabla_pagos').appendChild(tr);

}
function Modificar_Pago(json) {
    let form = document.forms['formulario_pagos'];
    form[0].value = json.id_cliente;
    form[1].value = json.forma_pago;
    form[2].value = json.fecha_pago;
    form[3].value = json.id_cliente;
    document.documentElement.scrollTop = 0;
}
function Eliminar_Pago(json) {

    let form = document.forms['formulario_pagos'];
    form[0].value = json.id_pago;
    var url = 'https://berajotweb.com/tzedakin/api/pago/' + form[0].value;
    fetch(url, {
        method: 'DELETE',
    })
        .then(response => response.json())
        .then(datos => {
            alert('Datos Eliminados');
            Limpiar_Tabla('pagos');
            Get_Pagos();
        })
        .catch(error => console.error("Error encontrado: ", error));
}




// ---------------------------------------- pedidos -------------------------
function validar_pedido() {
    let form = document.forms['formulario_pedidos'];
    let json = {
        "fecha_pedido": form[1].value,
        "id_estado": form[2].value,
        "id_cliente": form[3].value,
        "id_pago": form[4].value

    }
    Insert_Pedido(json);
    return false;
}
function Insert_Pedido(json) {
    var url = 'https://berajotweb.com/tzedakin/api/pedidos/';
    fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(json)
    })
        .then(response => response.json())
        .then(datos => {
            alert('registro exitoso');
            Limpiar_Tabla("pedidos");
            Get_Pedidos();
        })
        .catch(error => console.error("Error encontrado: ", error));
}
function Get_Pedidos() {
    var url = 'https://berajotweb.com/tzedakin/api/pedidos/';
    fetch(url, {
        method: 'GET'
    })
        .then(response => response.json())
        .then(datos => {
            Limpiar_Tabla("pedidos");
            for (let i = 0; i < datos.length; i++) {
                setTimeout(Tabla_Pedidos, 100 * 1, datos[i], i);
            }
            Get_Select();
            Get_Select_Pago();
        })
        .catch(error => console.error("Error encontrado: ", error));
}
function Get_Select() {
    var url = 'https://berajotweb.com/tzedakin/api/estados';
    fetch(url, {
        method: 'GET'
    })
        .then(response => response.json())
        .then(datos => {
            Eliminar_Select();
            for (let i = 0; i < datos.length; i++) {
                setTimeout(Seleccion_Estado, 100 * 1, datos[i], i);
            }
        })
        .catch(error => console.error("Error encontrado: ", error));
}

function Seleccion_Estado(datos) {
    let option = document.createElement('option');
    option.value = datos.id_estado;
    option.innerHTML = datos.estado;
    document.getElementById('estados').appendChild(option);
}
function Eliminar_Select() {
    let estado = document.getElementById('estados');
    let ultimo = estado.lastElementChild;
    while (ultimo) {
        estado.removeChild(ultimo);
        ultimo = estado.lastElementChild;
    }
    let option = document.createElement('option');
    option.innerHTML = "Seleccione Estado";
    estado.appendChild(option);
}
function Put_Pedido() {
    let form = document.forms['formulario_pedidos'];
    let json = {
        "fecha_pedido": form[1].value,
        "id_estado": form[2].value,
        "id_cliente": form[3].value,
        "id_pago": form[4].value

    }
    var url = 'https://berajotweb.com/tzedakin/api/pedidos/' + form[0].value;
    fetch(url, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(json)
    })
        .then(response => response.json())
        .then(datos => {
            alert('Datos Actualizados');
            Limpiar_Tabla('pedidos');
            Get_Pedidos();
            Get_Select();
        })
        .catch(error => console.error("Error encontrado: ", error));
}

function Tabla_Pedidos(datos, cantidad) {
    let arreglo = [cantidad + 1, datos.fecha_pedido, datos.id_estado, datos.id_cliente, datos.id_pago, "<a class='btn btn-success' onclick='Modificar_Pedido(" + JSON.stringify(datos) + ")'>Modificar</a>", "<a class='btn btn-danger' onclick='Eliminar_Pedido(" + JSON.stringify(datos) + ")'>Eliminar</a>"];
    let tr = document.createElement('tr');
    for (let x = 0; x < arreglo.length; x++) {
        let td = document.createElement('td');
        td.innerHTML = arreglo[x];
        tr.appendChild(td);
    }
    document.getElementById('tabla_pedidos').appendChild(tr);

}
function Modificar_Pedido(json) {
    let form = document.forms['formulario_pedidos'];
    form[0].value = json.id_pedido;
    form[1].value = json.fecha_pedido;
    form[2].value = json.estado;
    form[3].value = json.id_cliente;
    form[4].value = json.id_pago;
    document.documentElement.scrollTop = 0;
}
function Eliminar_Pedido(json) {

    let form = document.forms['formulario_pedidos'];
    form[0].value = json.id_pedido;
    var url = 'https://berajotweb.com/tzedakin/api/pedidos/' + form[0].value;
    fetch(url, {
        method: 'DELETE',
    })
        .then(response => response.json())
        .then(datos => {
            alert('Datos Eliminados');
            Limpiar_Tabla('pedidos');
            Get_Pedidos();
        })
        .catch(error => console.error("Error encontrado: ", error));
}


// ---------------------------------------- gama -------------------------
function validar_gama() {
    let form = document.forms['formulario_gama'];
    let json = {
        "gama": form[1].value,
    }
    Insert_Gama(json);
    return false;
}
function Insert_Gama(json) {
    var url = 'https://berajotweb.com/tzedakin/api/gama_producto/';
    fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(json)
    })
        .then(response => response.json())
        .then(datos => {
            alert('registro exitoso');
            Limpiar_Tabla("gama");
            Get_Gama();
        })
        .catch(error => console.error("Error encontrado: ", error));
}
function Get_Gama() {
    var url = 'https://berajotweb.com/tzedakin/api/gama_producto/';
    fetch(url, {
        method: 'GET'
    })
        .then(response => response.json())
        .then(datos => {
            Limpiar_Tabla("gama");
            for (let i = 0; i < datos.length; i++) {
                setTimeout(Tabla_Gama, 100 * 1, datos[i], i);
            }
        })
        .catch(error => console.error("Error encontrado: ", error));
}
function Put_Gama() {
    let form = document.forms['formulario_gama'];
    let json = {
        "gama": form[1].value,
    }
    var url = 'https://berajotweb.com/tzedakin/api/gama_producto/' + form[0].value;
    fetch(url, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(json)
    })
        .then(response => response.json())
        .then(datos => {
            alert('Datos Actualizados');
            Limpiar_Tabla('gama');
            Get_Gama();
        })
        .catch(error => console.error("Error encontrado: ", error));
}

function Tabla_Gama(datos, cantidad) {

    let arreglo = [cantidad + 1, datos.gama, "<a class='btn btn-success' onclick='Modificar_Gama(" + JSON.stringify(datos) + ")'>Modificar</a>", "<a class='btn btn-danger' onclick='Eliminar_Gama(" + JSON.stringify(datos) + ")'>Eliminar</a>"];
    let tr = document.createElement('tr');
    for (let x = 0; x < arreglo.length; x++) {
        let td = document.createElement('td');
        td.innerHTML = arreglo[x];
        tr.appendChild(td);
    }
    document.getElementById('tabla_gama').appendChild(tr);

}
function Modificar_Gama(json) {
    let form = document.forms['formulario_gama'];
    form[0].value = json.id_gama;
    form[1].value = json.gama;
    document.documentElement.scrollTop = 0;
}
function Eliminar_Gama(json) {

    let form = document.forms['formulario_gama'];
    form[0].value = json.id_gama;
    var url = 'https://berajotweb.com/tzedakin/api/gama_producto/' + form[0].value;
    fetch(url, {
        method: 'DELETE',
    })
        .then(response => response.json())
        .then(datos => {
            alert('Datos Eliminados');
            Limpiar_Tabla('gama');
            Get_Gama();
        })
        .catch(error => console.error("Error encontrado: ", error));
}



// ---------------------------------------- productos -------------------------
function validar_producto() {
    let form = document.forms['formulario_productos'];
    let json = {
        "nombre": form[1].value,
        "descripcion": form[2].value,
        "stock": form[3].value,
        "precio": form[4].value,
        "id_gama": form[5].value,
        "urlimagen": form[6].value,
    }
    Insert_Producto(json);
    return false;
}
function Insert_Producto(json) {
    var url = 'https://berajotweb.com/tzedakin/api/productos/';
    fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(json)
    })
        .then(response => response.json())
        .then(datos => {
            alert('registro exitoso');
            Limpiar_Tabla("productos");
            Get_Producto();
        })
        .catch(error => console.error("Error encontrado: ", error));
}
function Get_Producto() {
    var url = 'https://berajotweb.com/tzedakin/api/productos_inner/';
    fetch(url, {
        method: 'GET'
    })
        .then(response => response.json())
        .then(datos => {
            if (datos.length > 0) {
                Limpiar_Tabla("productos");
                for (let i = 0; i < datos.length; i++) {
                    setTimeout(Tabla_Productos, 100 * 1, datos[i], i);
                };
            }
        })
        .catch(error => console.error("Error encontrado: ", error));
}

function Put_Producto() {
    let form = document.forms['formulario_productos'];
    let json = {
        "nombre": form[1].value,
        "descripcion": form[2].value,
        "stock": form[3].value,
        "precio": form[4].value,
        "id_gama": form[5].value,
        "urlimagen": form[6].value,
    }
    var url = 'https://berajotweb.com/tzedakin/api/productos/' + form[0].value;
    fetch(url, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(json)
    })
        .then(response => response.json())
        .then(datos => {
            alert('Datos Actualizados');
            Limpiar_Tabla('productos');
            Get_Producto();
        })

        .catch(error => console.error("Error encontrado: ", error));
}

function Tabla_Productos(datos, cantidad) {

    var boton_autorizacion;
    datos.id_estado == 1 ? boton_autorizacion = "<a class='btn btn-success' onclick='Autorizacion_Producto(" + 2 + ',' + JSON.stringify(datos.correo) + ',' + JSON.stringify(datos.id_producto) + ")'>Autorizacion_Producto</a>" : boton_autorizacion = "<a class='btn btn-secondary' onclick='Autorizacion_Producto(" + 1 + ',' + JSON.stringify(datos.correo) + ',' + JSON.stringify(datos.id_producto) + ")'>Rechazar_Producto</a>"

    let arreglo = [cantidad + 1, datos.nombre, datos.descripcion, datos.precio, datos.categoria, datos.estado, datos.img_blob, datos.id_producto, datos.nombres + " " + datos.apellidos, datos.correo, boton_autorizacion, "<a class='btn btn-danger' onclick='Eliminar_Producto(" + JSON.stringify(datos) + ")'>Eliminar</a>"];
    let tr = document.createElement('tr');
    let img = document.createElement('img');
    for (let x = 0; x < arreglo.length; x++) {
        let td = document.createElement('td');
        if (x == 6) {
            img.src = arreglo[x];
            img.width = 100;
            img.height = 100;
            td.appendChild(img);

        } else {
            td.innerHTML = arreglo[x];
        }
        tr.appendChild(td);
    }
    document.getElementById('tabla_productos').appendChild(tr);

}


function Autorizacion_Producto(aceptacion, correo, id_producto) {
    var json_put = {
        "id_estado": aceptacion
    }
    var url = 'https://berajotweb.com/tzedakin/api/producto_autorizacion/' + id_producto;
    fetch(url, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(json_put)
    })
        .then(response => response.json())
        .then(datos => {
            if (datos.affectedRows == 1) {
                Autorizacion_Producto_Email(aceptacion, correo)
            } else if (datos.affectedRows == 0) {
                alert("Error: Ócurrio un problema con la aceptación");
            }
        })
        .catch(error => console.error("Error encontrado: ", error));
}


function Autorizacion_Producto_Email(aceptacion, correo) {

    var mensaje;
    aceptacion == 1 ? mensaje = "Su producto fue rechazado por infringir nuestras normas.!" : mensaje = "Se acepto su producto recargue la tienda de la aplicacion para poder verla"

    var json_email = {
        "from": "berajot@jorgehernandezr.dev",
        "to": correo,
        "subject": "Publicacion de producto",
        "text": mensaje
    }
    var url = 'https://berajotweb.com/tzedakin/api/correo_aceptar_producto';
    fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(json_email)
    })
        .then(response => {
            if (response.ok) {
                alert('Datos Actualizados. Se envio un correo al cliente.!');
                Limpiar_Tabla('productos');
                Get_Producto();
            }
        })
        .catch(error => console.error("Error encontrado: ", error));
}

function Eliminar_Producto(json) {

    let form = document.forms['formulario_productos'];
    form[0].value = json.id_producto;
    var url = 'https://berajotweb.com/tzedakin/api/productos/' + form[0].value;
    fetch(url, {
        method: 'DELETE',
    })
        .then(response => response.json())
        .then(datos => {
            alert('Datos Eliminados');
            Limpiar_Tabla('productos');
            Get_Producto();
        })
        .catch(error => console.error("Error encontrado: ", error));
}



// ---------------------------------------- detalles -------------------------
function validar_detalle() {
    let form = document.forms['formulario_detalles'];
    let json = {
        "cantidad": form[1].value,
        "precio_unidad": form[2].value,
        "comentario": form[3].value,
        "id_pedido": form[4].value,
        "id_producto": form[5].value,

    }
    Insert_Detalle(json);
    return false;
}
function Insert_Detalle(json) {
    var url = 'https://berajotweb.com/tzedakin/api/detalle_pedido/';
    fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(json)
    })
        .then(response => response.json())
        .then(datos => {
            alert('registro exitoso');
            Limpiar_Tabla("detalles");
            Get_Detalle();
        })
        .catch(error => console.error("Error encontrado: ", error));
}
function Get_Detalle() {
    var url = 'https://berajotweb.com/tzedakin/api/detalle_pedido/';
    fetch(url, {
        method: 'GET'
    })
        .then(response => response.json())
        .then(datos => {
            Limpiar_Tabla("detalles");
            for (let i = 0; i < datos.length; i++) {
                setTimeout(Tabla_Detalles, 100 * 1, datos[i], i);
            }
        })
        .catch(error => console.error("Error encontrado: ", error));
}
function Put_Detalle() {
    let form = document.forms['formulario_detalles'];
    let json = {
        "cantidad": form[1].value,
        "precio_unidad": form[2].value,
        "comentario": form[3].value,
        "id_pedido": form[4].value,
        "id_producto": form[5].value,
    }
    var url = 'https://berajotweb.com/tzedakin/api/detalle_pedido/' + form[0].value;
    fetch(url, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(json)
    })
        .then(response => response.json())
        .then(datos => {
            alert('Datos Actualizados');
            Limpiar_Tabla('detalles');
            Get_Detalle();
        })
        .catch(error => console.error("Error encontrado: ", error));
}

function Tabla_Detalles(datos, cantidad) {
    let arreglo = [cantidad + 1, datos.cantidad, datos.precio_unidad, datos.comentario, datos.id_pedido, datos.id_producto, "<a class='btn btn-success' onclick='Modificar_Detalle(" + JSON.stringify(datos) + ")'>Modificar</a>", "<a class='btn btn-danger' onclick='Eliminar_Detalle(" + JSON.stringify(datos) + ")'>Eliminar</a>"];
    let tr = document.createElement('tr');
    for (let x = 0; x < arreglo.length; x++) {
        let td = document.createElement('td');
        td.innerHTML = arreglo[x];
        tr.appendChild(td);
    }
    document.getElementById('tabla_detalles').appendChild(tr);

}
function Modificar_Detalle(json) {
    let form = document.forms['formulario_detalles'];
    form[0].value = json.id_detalle;
    form[1].value = json.cantidad;
    form[2].value = json.precio_unidad;
    form[3].value = json.comentario;
    form[4].value = json.id_pedido;
    form[5].value = json.id_producto;
    document.documentElement.scrollTop = 0;
}
function Eliminar_Detalle(json) {

    let form = document.forms['formulario_detalles'];
    form[0].value = json.id_detalle;
    var url = 'https://berajotweb.com/tzedakin/api/detalle_pedido/' + form[0].value;
    fetch(url, {
        method: 'DELETE',
    })
        .then(response => response.json())
        .then(datos => {
            alert('Datos Eliminados');
            Limpiar_Tabla('detalles');
            Get_Detalle();
        })
        .catch(error => console.error("Error encontrado: ", error));
}


// ------------------------------------------ Cursos -------------------------- //
function Mostrar_Video() {
    const form = document.forms['formulario_cursos'];
    const reproductor_video = document.getElementById('curso_video');
    form[1].value = form[1].value.replace(/view(.*)/, "preview");
    reproductor_video.setAttribute("src", form[1].value);
    reproductor_video.src = form[1].value;
}
function Validar_Curso() {
    const form = document.forms['formulario_cursos'];
    json = {
        "url_video": form[1].value,
        "nombre_curso": form[2].value,
        "descripcion": form[3].value
    };
    Insertar_Curso(json);
    return false;
}
function Insertar_Curso(json) {
    var url = 'https://berajotweb.com/tzedakin/api/cursos/';
    fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(json)
    })
        .then(response => response.json())
        .then(datos => {
            alert('registro exitoso.!');
            Limpiar_Tabla("tabla_cursos");
            Limpiar_Formulario_Cursos();
            Get_Cursos();
        })
        .catch(error => console.error("Error encontrado: ", error));
}
function Get_Cursos() {
    var url = 'https://berajotweb.com/tzedakin/api/cursos/';
    fetch(url, {
        method: 'GET'
    })
        .then(response => response.json())
        .then(datos => {
            if (datos.length > 0) {
                Limpiar_Tabla("tabla_cursos");
                for (let i = 0; i < datos.length; i++) {
                    setTimeout(Tabla_Cursos, 100 * 1, datos[i], i);
                }
            }

        })
        .catch(error => console.error("Error encontrado: ", error));
}
function Tabla_Cursos(datos, cantidad) {
    let arreglo = [cantidad + 1, datos.nombre_curso, datos.descripcion, '<iframe class="ratio ratio-16x9" src="' + datos.url_video + '" title="video_cursos" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share" allowfullscreen id="curso_video"></iframe>', "<a class='btn btn-success' onclick='Modificar_Curso(" + JSON.stringify(datos) + ")'>Modificar</a>", "<a class='btn btn-danger' onclick='Eliminar_Curso(" + JSON.stringify(datos) + ")'>Eliminar</a>"];
    let tr = document.createElement('tr');
    for (let x = 0; x < arreglo.length; x++) {
        let td = document.createElement('td');
        td.innerHTML = arreglo[x];
        tr.appendChild(td);
    }
    document.getElementById('tabla_cursos').appendChild(tr);

}
function Modificar_Curso(json) {
    const form = document.forms['formulario_cursos'];
    document.getElementById('btn_curso').setAttribute("disabled", true);

    const reproductor_video = document.getElementById('curso_video');
    form[1].value = json.url_video;
    reproductor_video.setAttribute("src", form[1].value);
    reproductor_video.src = form[1].value;

    form[2].value = json.nombre_curso;
    form[3].value = json.descripcion;

    document.documentElement.scrollTop = 0;
}
function Limpiar_Formulario_Cursos() {
    let form = document.forms['formulario_cursos'];
    for (let i = 0; i < form.length; i++) {
        form[i].value = "";
    }
    const reproductor_video = document.getElementById('curso_video');
    reproductor_video.src = "";
    document.getElementById('btn_curso').removeAttribute('disabled');
}
/// LIMPIAR TABLA
function Limpiar_Tabla(opcion) {
    if (opcion == 'pais') {
        var tabla = document.getElementById('tabla_paises');
        var ultimo = tabla.lastElementChild;
        while (ultimo) {
            tabla.removeChild(ultimo);
            ultimo = tabla.lastElementChild;
        }
    } else if (opcion == 'empleados') {
        var tabla = document.getElementById('tabla_empleados');
        var ultimo = tabla.lastElementChild;
        while (ultimo) {
            tabla.removeChild(ultimo);
            ultimo = tabla.lastElementChild;
        }
    }
    else if (opcion == 'clientes') {
        var tabla = document.getElementById('tabla_clientes');
        var ultimo = tabla.lastElementChild;
        while (ultimo) {
            tabla.removeChild(ultimo);
            ultimo = tabla.lastElementChild;
        }
    }
    else if (opcion == 'pagos') {
        var tabla = document.getElementById('tabla_pagos');
        var ultimo = tabla.lastElementChild;
        while (ultimo) {
            tabla.removeChild(ultimo);
            ultimo = tabla.lastElementChild;
        }
    }
    else if (opcion == 'pedidos') {
        var tabla = document.getElementById('tabla_pedidos');
        var ultimo = tabla.lastElementChild;
        while (ultimo) {
            tabla.removeChild(ultimo);
            ultimo = tabla.lastElementChild;
        }
    }
    else if (opcion == 'ciudad') {
        var tabla = document.getElementById('tabla_ciudades');
        var ultimo = tabla.lastElementChild;
        while (ultimo) {
            tabla.removeChild(ultimo);
            ultimo = tabla.lastElementChild;
        }
    }
    else if (opcion == 'productos') {
        var tabla = document.getElementById('tabla_productos');
        var ultimo = tabla.lastElementChild;
        while (ultimo) {
            tabla.removeChild(ultimo);
            ultimo = tabla.lastElementChild;
        }
    }
    else if (opcion == 'detalles') {
        var tabla = document.getElementById('tabla_detalles');
        var ultimo = tabla.lastElementChild;
        while (ultimo) {
            tabla.removeChild(ultimo);
            ultimo = tabla.lastElementChild;
        }
    }
    else if (opcion == 'subscriptores') {
        var tabla = document.getElementById('tabla_subscriptores');
        var ultimo = tabla.lastElementChild;
        while (ultimo) {
            tabla.removeChild(ultimo);
            ultimo = tabla.lastElementChild;
        }
    }
}



function Mostrar_Password_Empleado() {
    var cambio = document.getElementById("pass");
    if (cambio.type == "password") {
        cambio.type = "text";
    } else {
        cambio.type = "password";
    }
}

function Mostrar_Password_Cliente() {
    var cambio = document.getElementById("pass_cliente");
    if (cambio.type == "password") {
        cambio.type = "text";
    } else {
        cambio.type = "password";
    }
}