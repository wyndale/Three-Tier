const API_BASE = 'https://localhost:5005/api';

// Initial Load
window.addEventListener('load', () => {
    loadProducts();
    loadCategories();
});

// Product Management
async function loadProducts() {
    try {
        const response = await fetch(`${API_BASE}/product`);
        const products = await response.json();
        displayProducts(products);
    } catch (error) {
        showError('Failed to load products');
    }
}

function displayProducts(products) {
    const container = document.getElementById('productsContainer');
    container.innerHTML = products.map(product => `
        <div class="card">
            <h3>${product.name}</h3>
            <p style="margin: 0.8rem 0;">${product.description}</p>
            <div style="display: flex; justify-content: space-between; align-items: center;">
                <div>
                    <span style="font-size: 1.2rem; color: var(--primary);">$${product.price.toFixed(2)}</span>
                    <div class="category-item">${product.categoryName}</div>
                </div>
                <div style="display: flex; gap: 0.5rem;">
                    <button class="success" onclick="editProduct('${product.id}')">Edit</button>
                    <button class="danger" onclick="deleteProduct('${product.id}')">Delete</button>
                </div>
            </div>
        </div>
    `).join('');
}

// Category Management
async function loadCategories() {
    try {
        const response = await fetch(`${API_BASE}/category`);
        const categories = await response.json();
        displayCategories(categories);
        populateCategorySelect(categories);
    } catch (error) {
        showError('Failed to load categories');
    }
}

function displayCategories(categories) {
    const container = document.getElementById('categoriesList');
    container.innerHTML = categories.map(category => `
        <div class="category-item">
            <span>${category.name}</span>
            <button class="danger" style="padding: 0.1rem 0.1rem; width: 2rem; height: 2rem; 
            border-radius: 50%; font-weight: 600; margin-right: -10px;" 
                    onclick="deleteCategory('${category.id}')">×</button>
        </div>
    `).join('');
}

// Modal Handling
async function showProductModal(productId = null) {
    const modal = document.getElementById('productModal');
    modal.style.display = 'flex';
    
    if (productId) {
        try {
            const response = await fetch(`${API_BASE}/product/${productId}`);
            const product = await response.json();
            document.getElementById('productId').value = product.id;
            document.getElementById('productName').value = product.name;
            document.getElementById('productDescription').value = product.description;
            document.getElementById('productPrice').value = product.price;
            document.getElementById('productCategory').value = product.categoryId;
        } catch (error) {
            showError('Failed to load product');
        }
    }
}

function closeModal() {
    document.getElementById('productModal').style.display = 'none';
    document.getElementById('productForm').reset();
}

// Form Handling
async function handleProductSubmit(e) {
    e.preventDefault();
    
    const productData = {
        name: document.getElementById('productName').value,
        description: document.getElementById('productDescription').value,
        price: parseFloat(document.getElementById('productPrice').value),
        categoryName: document.getElementById('productCategory').selectedOptions[0].text
    };

    const productId = document.getElementById('productId').value;
    const method = productId ? 'PUT' : 'POST';
    const url = productId ? `${API_BASE}/product/${productId}` : `${API_BASE}/product`;

    try {
        const response = await fetch(url, {
            method,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(productData)
        });
        
        if (!response.ok) throw new Error();
        
        closeModal();
        loadProducts();
    } catch (error) {
        showError(productId ? 'Failed to update product' : 'Failed to create product');
    }
}

// Function to handle product editing
async function editProduct(productId) {
    try {
        // Fetch product details
        const response = await fetch(`${API_BASE}/product/${productId}`);
        const product = await response.json();
        
        // Populate modal with product data
        document.getElementById('productId').value = product.id;
        document.getElementById('productName').value = product.name;
        document.getElementById('productDescription').value = product.description;
        document.getElementById('productPrice').value = product.price;
        
        // Set category selection
        const categorySelect = document.getElementById('productCategory');
        const categoryOption = Array.from(categorySelect.options).find(
            option => option.text === product.categoryName
        );
        if (categoryOption) {
            categoryOption.selected = true;
        }
        
        // Show the modal
        showProductModal();
    } catch (error) {
        showError('Failed to load product for editing');
    }
}

async function addCategory() {
    const name = document.getElementById('newCategory').value;
    if (!name) return;

    try {
        await fetch(`${API_BASE}/category`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ name })
        });
        document.getElementById('newCategory').value = '';
        loadCategories();
    } catch (error) {
        showError('Failed to add category');
    }
}

// CRUD Operations
async function deleteProduct(id) {
    if (confirm('Are you sure you want to delete this product?')) {
        try {
            await fetch(`${API_BASE}/product/${id}`, { method: 'DELETE' });
            loadProducts();
        } catch (error) {
            showError('Failed to delete product');
        }
    }
}

async function deleteCategory(id) {
    if (confirm('Are you sure you want to delete this category?')) {
        try {
            await fetch(`${API_BASE}/category/${id}`, { method: 'DELETE' });
            loadCategories();
        } catch (error) {
            showError('Failed to delete category');
        }
    }
}

// Helpers
function populateCategorySelect(categories) {
    const select = document.getElementById('productCategory');
    select.innerHTML = categories.map(c => 
        `<option value="${c.id}">${c.name}</option>`
    ).join('');
}

function showError(message) {
    alert(message);
}