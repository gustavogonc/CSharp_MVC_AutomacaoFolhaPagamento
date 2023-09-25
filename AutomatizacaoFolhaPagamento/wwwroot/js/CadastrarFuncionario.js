document.getElementById('cpf').addEventListener('input', maskCPF);

function maskCPF(e) {
    var cpf = e.target.value;

    // Removendo todos os caracteres exceto números
    if (e.target.value.length <= 14) {
        cpf = cpf.replace(/\D/g, '');

        // Adicionando pontos e hífen
        cpf = cpf.replace(/(\d{3})(\d)/, '$1.$2');
        cpf = cpf.replace(/(\d{3})(\d)/, '$1.$2');
        cpf = cpf.replace(/(\d{3})(\d{1,2})$/, '$1-$2');

        e.target.value = cpf;
    }
    else {return }
}

