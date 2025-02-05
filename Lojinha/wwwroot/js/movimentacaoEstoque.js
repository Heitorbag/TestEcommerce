const moviEstoqueApiUrl = 'https://localhost:7217/api/MovimentacaoEstoque';

document.addEventListener('DOMContentLoaded', () => {
    getMovimentacoes();

    document.getElementById('moviEstoqueForm').addEventListener('submit', addMovimentacao);
    document.getElementById('editMoviEstoqueForm').addEventListener('submit', updateMovimentacao);
});

function getMovimentacoes() {
    fetch(moviEstoqueApiUrl)
        .then(response => response.json())
        .then(data => displayMovimentacoes(data))
        .catch(error => console.error('Erro ao obter movimentações:', error));
}

function addMovimentacao(event) {
    event.preventDefault();

    const movimentacao = {
        idProduto: document.getElementById('idProdutoMovi').value.trim(),
        tipoMovimentacao: document.getElementById('tipoMovi').value.trim(),
        quantidade: document.getElementById('quantidadeMovi').value.trim(),
        idFornecedor: document.getElementById('idFornecedorMovi').value.trim(),
    };

    fetch(moviEstoqueApiUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(movimentacao),
    })
        .then(() => {
            getMovimentacoes();
            document.getElementById('moviEstoqueForm').reset();
        })
        .catch(error => console.error('Erro ao adicionar movimentação:', error));
}

function updateMovimentacao(event) {
    event.preventDefault();

    const idMovimentacao = document.getElementById('edit-idMovimentacao').value;
    const movimentacao = {
        idMovimentacao: parseInt(idMovimentacao, 10),
        idProduto: document.getElementById('edit-idProdutoMovi').value.trim(),
        tipoMovimentacao: document.getElementById('edit-tipoMovi').value.trim(),
        quantidade: document.getElementById('edit-quantidadeMovi').value.trim(),
        idFornecedor: document.getElementById('edit-idFornecedorMovi').value.trim(),
    };

    fetch(`${moviEstoqueApiUrl}/${idMovimentacao}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(movimentacao),
    })
        .then(() => {
            getMovimentacoes();
            closeEditMoviEstoqueForm();
        })
        .catch(error => console.error('Erro ao atualizar movimentação:', error));
}

function deleteMovimentacao(id) {
    fetch(`${moviEstoqueApiUrl}/${id}`, {
        method: 'DELETE',
    })
        .then(() => getMovimentacoes())
        .catch(error => console.error('Erro ao excluir movimentação:', error));
}

function displayMovimentacoes(movimentacoes) {
    const tableBody = document.getElementById('moviEstoqueTable');
    tableBody.innerHTML = '';

    movimentacoes.forEach(movimentacao => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${movimentacao.idMovimentacao}</td>
            <td>${movimentacao.idProduto}</td>
            <td>${movimentacao.tipoMovimentacao}</td>
            <td>${movimentacao.quantidade}</td>
            <td>${movimentacao.idFornecedor}</td>
            <td>${movimentacao.dataMovimentacao}</td>
            <td>
                <button onclick="showEditMoviEstoqueForm(${movimentacao.idMovimentacao}, '${movimentacao.idProduto}', '${movimentacao.tipoMovimentacao}', ${movimentacao.quantidade}, ${movimentacao.idFornecedor})">Editar</button>
                <button onclick="deleteMovimentacao(${movimentacao.idMovimentacao})">Excluir</button>
            </td>
        `;
        tableBody.appendChild(row);
    });
}

function showEditMoviEstoqueForm(idMovimentacao, idProduto, tipoMovimentacao, quantidade, idFornecedor) {
    document.getElementById('edit-idMovimentacao').value = idMovimentacao;
    document.getElementById('edit-idProdutoMovi').value = idProduto;
    document.getElementById('edit-tipoMovi').value = tipoMovimentacao;
    document.getElementById('edit-quantidadeMovi').value = quantidade;
    document.getElementById('edit-idFornecedorMovi').value = idFornecedor;

    document.getElementById('editMoviEstoqueForm').classList.remove('hidden');
}

function closeEditMoviEstoqueForm() {
    document.getElementById('editMoviEstoqueForm').classList.add('hidden');
}
