// Authentication and Authorization logic

const Auth = {
    async login(email, password, errorDivId, successDivId) {
        const errorDiv = document.getElementById(errorDivId);
        const successDiv = document.getElementById(successDivId);
        
        errorDiv.classList.add('d-none');
        if (successDiv) successDiv.classList.add('d-none');
        
        try {
            const response = await fetch('/api/Auth/login', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ email, password })
            });
            
            const data = await response.json();
            
            if (response.ok) {
                if (successDiv) successDiv.classList.remove('d-none');
                localStorage.setItem('userRole', data.userRole || data.UserRole);
                setTimeout(() => {
                    window.location.href = '/Home/Index';
                }, 500);
            } else {
                errorDiv.textContent = data.message || data.Message || 'Login failed';
                errorDiv.classList.remove('d-none');
            }
        } catch (error) {
            errorDiv.textContent = 'Technical error: ' + error.message;
            errorDiv.classList.remove('d-none');
        }
    },

    async register(formData, errorDivId, successDivId, formId) {
        const errorDiv = document.getElementById(errorDivId);
        const successDiv = document.getElementById(successDivId);
        
        errorDiv.classList.add('d-none');
        successDiv.classList.add('d-none');
        
        try {
            const response = await fetch('/api/Auth/register', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(formData)
            });
            
            const data = await response.json();
            
            if (response.ok) {
                successDiv.textContent = data.message || 'Registration successful!';
                successDiv.classList.remove('d-none');
                document.getElementById(formId).reset();
            } else {
                errorDiv.textContent = data.message || 'Registration failed';
                errorDiv.classList.remove('d-none');
            }
        } catch (error) {
            errorDiv.textContent = 'Technical error: ' + error.message;
            errorDiv.classList.remove('d-none');
        }
    },

    async logout() {
        try {
            const response = await fetch('/api/Auth/logout', { method: 'POST' });
            if (response.ok) {
                localStorage.removeItem('userRole');
                window.location.href = '/Account/Login';
            }
        } catch (error) {
            console.error('Logout error:', error);
        }
    }
};