import React, { useEffect, useState } from "react";
import { useMsal, useIsAuthenticated } from "@azure/msal-react";
import { loginRequest } from "./authConfig";
import { fetchProducts, addProduct, updateProduct, deleteProduct } from "./services/productService";
import ProductViewModal from "./ProductViewModal"; 

interface Product {
    id: number;
    name: string;
    price: number;
    description: string;
}

function App() {
    const { instance, inProgress } = useMsal();
    const isAuthenticated = useIsAuthenticated();
    const [products, setProducts] = useState<Product[]>([]);
    const [editingProduct, setEditingProduct] = useState<Product | null>(null);
    const [newProduct, setNewProduct] = useState({ name: "", price: 0, description: "" });
    const [viewingProduct, setViewingProduct] = useState<Product | null>(null); 

    // Fetch products on load
    useEffect(() => {
        if (!isAuthenticated && inProgress === "none") {
            instance.loginPopup(loginRequest).catch((e) => console.error(e));
        }
    }, [isAuthenticated, inProgress, instance]);

    useEffect(() => {
        const loadProducts = async () => {
            try {
                const account = instance.getAllAccounts()[0];
                if (!account) return;
                const prods = await fetchProducts(instance, account);
                setProducts(prods);
            } catch (err) {
                console.error("Error loading products:", err);
            }
        };

        if (isAuthenticated && inProgress === "none") {
            loadProducts();
        }
    }, [isAuthenticated, inProgress, instance]);

    // Handle delete
    const handleDelete = async (id: number) => {
        const account = instance.getAllAccounts()[0];
        if (!account) return;

        try {
            await deleteProduct(instance, account, id);
            setProducts(products.filter(p => p.id !== id));
        } catch (err) {
            console.error("Delete failed:", err);
        }
    };

    // Handle edit form change
    const handleEditChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        if (!editingProduct) return;
        setEditingProduct({ ...editingProduct, [e.target.name]: e.target.value });
    };

    // Save edited product
    const handleEditSave = async () => {
        if (!editingProduct) return;
        const account = instance.getAllAccounts()[0];
        if (!account) return;

        try {
            const updated = await updateProduct(instance, account, editingProduct);
            setProducts(products.map(p => (p.id === updated.id ? updated : p)));
            setEditingProduct(null);
        } catch (err) {
            console.error("Update failed:", err);
        }
    };

    // Handle add form change
    const handleAddChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        setNewProduct({ ...newProduct, [e.target.name]: e.target.value });
    };

    // Add new product
    const handleAddProduct = async () => {
        const account = instance.getAllAccounts()[0];
        if (!account) return;

        try {
            const added = await addProduct(instance, account, newProduct);
            setProducts([...products, added]);
            setNewProduct({ name: "", price: 0, description: "" });
        } catch (err) {
            console.error("Add failed:", err);
        }
    };

    return (
        <div className="p-6 max-w-4xl mx-auto">
            <h1 className="text-3xl font-bold mb-6">Products</h1>

            {/* Add new product form */}
            <div className="mb-6 border p-4 rounded shadow-sm">
                <h2 className="text-xl font-semibold mb-2">Add Product</h2>
                <input
                    type="text"
                    name="name"
                    placeholder="Name"
                    value={newProduct.name}
                    onChange={handleAddChange}
                    className="border p-2 mr-2 rounded w-1/4"
                />
                <input
                    type="text"
                    name="price"
                    placeholder="Price"
                    value={newProduct.price}
                    onChange={handleAddChange}
                    className="border p-2 mr-2 rounded w-1/4"
                />
                <input
                    type="text"
                    name="description"
                    placeholder="Description"
                    value={newProduct.description}
                    onChange={handleAddChange}
                    className="border p-2 mr-2 rounded w-1/3"
                />
                <button onClick={handleAddProduct} className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700">
                    Add
                </button>
            </div>

            {/* Products table */}
            <table className="min-w-full table-auto border-collapse border border-gray-300">
                <thead>
                    <tr>
                        <th className="border border-gray-300 px-4 py-2 text-left">ID</th>
                        <th className="border border-gray-300 px-4 py-2 text-left">Name</th>
                        <th className="border border-gray-300 px-4 py-2 text-left">Price</th>
                        <th className="border border-gray-300 px-4 py-2 text-left">Description</th>
                        <th className="border border-gray-300 px-4 py-2">Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {products.map(p => (
                        <tr key={p.id} className="hover:bg-gray-100">
                            <td className="border border-gray-300 px-4 py-2">{p.id}</td>
                            <td className="border border-gray-300 px-4 py-2 w-1/6">
                                {editingProduct?.id === p.id ? (
                                    <input
                                        name="name"
                                        value={editingProduct.name}
                                        onChange={handleEditChange}
                                        className="border rounded p-1 w-full"
                                    />
                                ) : (
                                    p.name
                                )}
                            </td>
                            <td className="border border-gray-300 px-4 py-2 w-1/6">
                                {editingProduct?.id === p.id ? (
                                    <input
                                        name="price"
                                        value={editingProduct.price}
                                        onChange={handleEditChange}
                                        className="border rounded p-1 w-full"
                                    />
                                ) : (
                                    p.price
                                )}
                            </td>
                            <td className="border border-gray-300 px-4 py-2 w-1/6">
                                {editingProduct?.id === p.id ? (
                                    <input
                                        name="description"
                                        value={editingProduct.description}
                                        onChange={handleEditChange}
                                        className="border rounded p-1 w-full"
                                    />
                                ) : (
                                    p.description
                                )}
                            </td>
                            <td className="border border-gray-300 px-4 py-2 text-center space-x-2">
                                {editingProduct?.id === p.id ? (
                                    <>
                                        <button
                                            onClick={handleEditSave}
                                            className="bg-green-600 text-white px-3 py-1 rounded hover:bg-green-700"
                                        >
                                            Save
                                        </button>
                                        <button
                                            onClick={() => setEditingProduct(null)}
                                            className="bg-gray-400 text-white px-3 py-1 rounded hover:bg-gray-500"
                                        >
                                            Cancel
                                        </button>
                                    </>
                                ) : (
                                    <>
                                        <button
                                            onClick={() => setViewingProduct(p)}
                                            className="bg-blue-500 text-white px-3 py-1 rounded hover:bg-blue-600"
                                        >
                                            View
                                        </button>
                                        <button
                                            onClick={() => setEditingProduct(p)}
                                            className="bg-yellow-500 text-white px-3 py-1 rounded hover:bg-yellow-600"
                                        >
                                            Edit
                                        </button>
                                        <button
                                            onClick={() => handleDelete(p.id)}
                                            className="bg-red-600 text-white px-3 py-1 rounded hover:bg-red-700"
                                        >
                                            Delete
                                        </button>
                                    </>
                                )}
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
            {viewingProduct && (
                <ProductViewModal
                    product={viewingProduct}
                    onClose={() => setViewingProduct(null)}
                />
            )}
        </div>

    );
}

export default App;
