const apiUrl = 'https://localhost:7217/api/Usuario';

document.addEventListener('DOMContentLoaded', () => {
    getUsuarios();

    document.getElementById('usuarioForm').addEventListener('submit', addUsuario);
    document.getElementById('editUsuarioForm').addEventListener('submit', updateUsuario);
});

function getUsuarios() {
    fetch(apiUrl)
        .then(response => response.json())
        .then(data => displayUsuarios(data))
        .catch(error => console.error('Erro ao obter usuários:', error));
}

function addUsuario(event) {
    event.preventDefault();

    const usuario = {
        nome: document.getElementById('nome').value.trim(),
        email: document.getElementById('email').value.trim(),
        endereco: document.getElementById('endereco').value.trim(),
    };

    fetch(apiUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(usuario),
    })
        .then(() => {
            getUsuarios();
            document.getElementById('usuarioForm').reset();
        })
        .catch(error => console.error('Erro ao adicionar usuário:', error));
}

function updateUsuario(event) {
    event.preventDefault();

    const id = document.getElementById('edit-id').value;
    const usuario = {
        id: parseInt(id, 10),
        nome: document.getElementById('edit-nome').value.trim(),
        email: document.getElementById('edit-email').value.trim(),
        endereco: document.getElementById('edit-endereco').value.trim(),
    };

    fetch(`${apiUrl}/${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(usuario),
    })
        .then(() => {
            getUsuarios();
            closeEditUsuarioForm();
        })
        .catch(error => console.error('Erro ao atualizar usuário:', error));
}

function deleteUsuario(id) {
    fetch(`${apiUrl}/${id}`, {
        method: 'DELETE',
    })
        .then(() => getUsuarios())
        .catch(error => console.error('Erro ao excluir usuário:', error));
}

function displayUsuarios(usuarios) {
    const tableBody = document.getElementById('usuarioTable');
    tableBody.innerHTML = '';

    usuarios.forEach(usuario => {
        const row = document.createElement('tr');

        row.innerHTML = `
            <td>${usuario.id}</td>
            <td>${usuario.nome}</td>
            <td>${usuario.email}</td>
            <td>${usuario.endereco}</td>
            <td>
                <button onclick="showEditUsuarioForm(${usuario.id}, '${usuario.nome}', '${usuario.email}', '${usuario.endereco}')">Editar</button>
                <button onclick="deleteUsuario(${usuario.id})">Excluir</button>
            </td>
        `;

        tableBody.appendChild(row);
    });
}

function showEditUsuarioForm(id, nome, email, endereco) {
    document.getElementById('edit-id').value = id;
    document.getElementById('edit-nome').value = nome;
    document.getElementById('edit-email').value = email;
    document.getElementById('edit-endereco').value = endereco;

    document.getElementById('editUsuarioForm').classList.remove('hidden');
}

function closeEditUsuarioForm() {
    document.getElementById('editUsuarioForm').classList.add('hidden');
}
