import React, { useEffect, useState } from "react";
import { useMsal, useIsAuthenticated } from "@azure/msal-react";
import { loginRequest } from "./authConfig";
import { fetchProducts } from "./services/productService";

function App() {
    const { instance, inProgress } = useMsal();
    const isAuthenticated = useIsAuthenticated();
    const [products, setProducts] = useState<any[]>([]);

    useEffect(() => {
        if (!isAuthenticated && inProgress === "none") {
            instance.loginPopup(loginRequest).catch((e) => {
                console.error(e);
            });
        }
    }, [isAuthenticated, inProgress, instance]);

    useEffect(() => {
        const loadProducts = async () => {
            try {
                const account = instance.getAllAccounts()[0];
                if (!account) return;

                const products = await fetchProducts(instance, account);
                setProducts(products);
            } catch (err) {
                console.error("Error loading products:", err);
            }
        };

        if (isAuthenticated && inProgress === "none") {
            loadProducts();
        }
    }, [isAuthenticated, inProgress, instance]);

    return (
        <div>
            <h1>Products</h1>
            {products.map((p, i) => (
                <div key={i}>{p.name}</div>
            ))}
        </div>
    );
}

export default App;
