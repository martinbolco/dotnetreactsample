import { AccountInfo, IPublicClientApplication } from "@azure/msal-browser";
import { loginRequest } from "../authConfig";

const API_URL = process.env.REACT_APP_API_URL ?? "";

export const fetchProducts = async (
    msalInstance: IPublicClientApplication,
    account: AccountInfo
): Promise<any[]> => {
    const response = await msalInstance.acquireTokenSilent({ ...loginRequest, account });
    const token = response.accessToken;

    const res = await fetch(API_URL, {
        headers: { Authorization: `Bearer ${token}` },
    });

    if (!res.ok) {
        throw new Error("Failed to fetch products");
    }
    return await res.json();
};

export const fetchProduct = async (
    msalInstance: IPublicClientApplication,
    account: AccountInfo,
    id: number
): Promise<any> => {
    const response = await msalInstance.acquireTokenSilent({ ...loginRequest, account });
    const token = response.accessToken;

    const res = await fetch(`${API_URL}/${id}`, {
        headers: { Authorization: `Bearer ${token}` },
    });

    if (!res.ok) {
        throw new Error("Failed to fetch products");
    }
    return await res.json();
};

export const addProduct = async (
    msalInstance: IPublicClientApplication,
    account: AccountInfo,
    product: { name: string; price: number; description: string }
) => {
    const response = await msalInstance.acquireTokenSilent({ ...loginRequest, account });
    const token = response.accessToken;

    const res = await fetch(API_URL, {
        method: "POST",
        headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
        },
        body: JSON.stringify(product),
    });

    if (!res.ok) {
        throw new Error("Failed to add product");
    }

    return await res.json();
};

export const updateProduct = async (
    msalInstance: IPublicClientApplication,
    account: AccountInfo,
    product: { id: number; name: string; price: number; description: string }
) => {
    const response = await msalInstance.acquireTokenSilent({ ...loginRequest, account });
    const token = response.accessToken;

    const res = await fetch(`${API_URL}/${product.id}`, {
        method: "PUT",
        headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
        },
        body: JSON.stringify(product),
    });

    if (!res.ok) {
        throw new Error("Failed to update product");
    }

    return await res.json();
};

export const deleteProduct = async (
    msalInstance: IPublicClientApplication,
    account: AccountInfo,
    id: number
) => {
    const response = await msalInstance.acquireTokenSilent({ ...loginRequest, account });
    const token = response.accessToken;

    const res = await fetch(`${API_URL}/${id}`, {
        method: "DELETE",
        headers: { Authorization: `Bearer ${token}` },
    });

    if (!res.ok) {
        throw new Error("Failed to delete product");
    }

    return true;
};
