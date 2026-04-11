import { apiClient } from "@/lib/api-client";

export const getProfile = () => apiClient.get(`/userinfo`);
