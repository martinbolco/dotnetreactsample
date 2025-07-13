import React, { useEffect, useState } from "react";
import { useMsal } from "@azure/msal-react";
import { fetchProduct } from "./services/productService";


interface Product {
    id: number;
    name: string;
    price: number;
    description: string;
}

interface Props {
    product: Product;
    onClose: () => void;
}

const ProductViewModal: React.FC<Props> = ({ product, onClose }) => {
    const { instance } = useMsal();
    const [details, setDetails] = useState<any | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const loadProduct = async () => {
            setLoading(true);
            try {
                const account = instance.getAllAccounts()[0];
                if (!account) throw new Error("No account found");
                const freshProduct = await fetchProduct(instance, account, product.id);
                setDetails(freshProduct);
            } catch (err) {
                setError((err as Error).message);
            } finally {
                setLoading(false);
            }
        };

        loadProduct();
    }, [instance, product.id]);

    if (!product) return null;

    return (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
            <div className="bg-white rounded-lg p-6 w-96 shadow-xl relative">
                <button onClick={onClose} className="absolute top-2 right-2 text-gray-500 hover:text-black">✖</button>

                {loading && <p>Loading...</p>}
                {error && <p className="text-red-500">{error}</p>}

                {details && (
                    <>
                        <h2 className="text-xl font-bold mb-2">Product Details</h2>
                        <p><strong>ID:</strong> {details.id}</p>
                        <p><strong>Name:</strong> {details.name}</p>
                        <p><strong>Name:</strong> {details.price}</p>
                        <p><strong>Description:</strong> {details.description}</p>
                    </>
                )}
            </div>
        </div>
    );
};

export default ProductViewModal;