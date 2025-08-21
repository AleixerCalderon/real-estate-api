'use client';

import { useState, useEffect } from 'react';
import { Owner } from '@/types';
import { ownerApi, formatDate } from '@/services/api';
import { LoadingSpinner } from '@/components/LoadingSpinner';
import { ErrorMessage } from '@/components/ErrorMessage';
import { EmptyState } from '@/components/EmptyState';
import { User, Phone, MapPin, Calendar, Users, Plus } from 'lucide-react';
import { OwnerForm } from '@/components/OwnerForm';

export default function OwnersPage() {
  const [owners, setOwners] = useState<Owner[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showOwnerForm, setShowOwnerForm] = useState(false);

  useEffect(() => {
    loadOwners();
  }, []);

  const loadOwners = async () => {
    try {
      setLoading(true);
      setError(null);
      
      const response = await ownerApi.getAll();
      
      if (response.success && response.data) {
        setOwners(response.data);
      } else {
        setError(response.message || 'Error al cargar los propietarios');
      }
    } catch (err) {
      console.error('Error loading owners:', err);
      setError(err instanceof Error ? err.message : 'Error de conexión con el servidor');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <div className="mb-8 flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold text-gray-900 mb-2">
            👥 Propietarios Registrados
          </h1>
          <p className="text-gray-600">
            Lista completa de propietarios registrados en el sistema
          </p>
        </div>
        
        <button
          onClick={() => setShowOwnerForm(true)}
          className="bg-green-600 text-white px-6 py-3 rounded-lg hover:bg-green-700 transition-colors flex items-center gap-2 font-medium"
        >
          <Plus className="h-5 w-5" />
          Nuevo Propietario
        </button>
      </div>

      {error && (
        <ErrorMessage
          message={error}
          onRetry={loadOwners}
        />
      )}

      {loading && (
        <LoadingSpinner message="Cargando propietarios..." />
      )}

      {!loading && !error && owners.length === 0 && (
        <EmptyState
          title="No hay propietarios registrados"
          description="Aún no se han registrado propietarios en el sistema."
          icon={<Users className="h-24 w-24" />}
        />
      )}

      {!loading && !error && owners.length > 0 && (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {owners.map((owner) => (
            <div key={owner.id} className="bg-white rounded-lg shadow-md p-6 hover:shadow-lg transition-shadow animate-fade-in">
              <div className="flex items-center mb-4">
                <div className="bg-blue-100 rounded-full p-3 mr-4">
                  <User className="h-6 w-6 text-blue-600" />
                </div>
                <h3 className="text-lg font-semibold text-gray-900">
                  {owner.name}
                </h3>
              </div>
              
              <div className="space-y-3 text-sm text-gray-600">
                <div className="flex items-center gap-2">
                  <MapPin className="h-4 w-4 text-gray-400 flex-shrink-0" />
                  <span className="line-clamp-2">{owner.address}</span>
                </div>
                
                <div className="flex items-center gap-2">
                  <Phone className="h-4 w-4 text-gray-400 flex-shrink-0" />
                  <span>{owner.phone}</span>
                </div>
                
                <div className="flex items-center gap-2">
                  <Calendar className="h-4 w-4 text-gray-400 flex-shrink-0" />
                  <span>Nacimiento: {formatDate(owner.birthday)}</span>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}

      {showOwnerForm && (
        <OwnerForm
          onClose={() => setShowOwnerForm(false)}
          onSuccess={() => {
            loadOwners(); 
          }}
        />
      )}
    </div>
  );
}