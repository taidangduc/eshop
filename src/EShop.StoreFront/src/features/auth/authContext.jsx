import { createContext, useMemo } from "react";
import { useQuery, useQueryClient } from "@tanstack/react-query";
import { getProfile } from "./api";

const AuthContext = createContext(null);

const BFF_BASE_URL =
  import.meta.env.VITE_BFF_URL ||
  import.meta.env.REACT_APP_BFF_URL ||
  "https://localhost:5002/bff";

const getProfileSafe = async () => {
  try {
    const { data } = await getProfile();
    return data ?? null;
  } catch (error) {
    if (error?.response?.status === 401 || error?.status === 401) {
      return null;
    }
    return null;
  }
};

export const AuthProvider = ({ children }) => {
  const queryClient = useQueryClient();

  const {
    data: user,
    isLoading,
    isFetching,
    error,
    refetch,
  } = useQuery({
    queryKey: ["auth", "profile"],
    queryFn: getProfileSafe,
    staleTime: 60_000,
    retry: false,
  });

  const login = async () => {
    const returnUrl = encodeURIComponent(
      window.location.pathname + window.location.search,
    );
    window.location.href = `${BFF_BASE_URL}/login?returnUrl=${returnUrl}`;
    return { success: true };
  };

  const logout = async () => {
    queryClient.setQueryData(["auth", "profile"], null);
    const returnUrl = encodeURIComponent(window.location.origin);
    window.location.href = `${BFF_BASE_URL}/logout?returnUrl=${returnUrl}`;
    return { success: true };
  };

  const signup = async () => {
    const returnUrl = encodeURIComponent(
      window.location.pathname + window.location.search,
    );
    window.location.href = `${BFF_BASE_URL}/register?returnUrl=${returnUrl}`;
    return { success: true };
  };

  const value = useMemo(
    () => ({
      user,
      isAuthenticated: Boolean(user),
      isLoading,
      isFetching,
      error,
      login,
      logout,
      signup,
      refetchProfile: refetch,
    }),
    [user, isLoading, isFetching, error, refetch],
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};
export { AuthContext };