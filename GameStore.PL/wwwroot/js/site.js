(function () {
    const btn = document.getElementById('darkModeToggle');
    if (!btn) return;

    const applyDark = () => {
        document.body.classList.add('dark-mode');
        document.documentElement.setAttribute('data-theme', 'dark');
        localStorage.setItem('darkMode', 'enabled');
        btn.innerHTML = '<i class="fa-solid fa-sun"></i>';
        btn.classList.remove('btn-outline-light');
        btn.classList.add('btn-warning');
    };

    const removeDark = () => {
        document.body.classList.remove('dark-mode');
        document.documentElement.setAttribute('data-theme', 'light');
        localStorage.setItem('darkMode', 'disabled');
        btn.innerHTML = '<i class="fa-solid fa-moon"></i>';
        btn.classList.remove('btn-warning');
        btn.classList.add('btn-outline-light');
    };

    // init
    if (localStorage.getItem('darkMode') === 'enabled') applyDark(); else removeDark();

    btn.addEventListener('click', () => {
        if (localStorage.getItem('darkMode') !== 'enabled') applyDark(); else removeDark();
    });
})();