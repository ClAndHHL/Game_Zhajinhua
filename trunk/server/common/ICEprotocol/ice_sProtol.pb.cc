// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: ice_sProtol.proto

#define INTERNAL_SUPPRESS_PROTOBUF_FIELD_DEPRECATION
#include "ice_sProtol.pb.h"

#include <algorithm>

#include <google/protobuf/stubs/common.h>
#include <google/protobuf/stubs/once.h>
#include <google/protobuf/io/coded_stream.h>
#include <google/protobuf/wire_format_lite_inl.h>
#include <google/protobuf/descriptor.h>
#include <google/protobuf/generated_message_reflection.h>
#include <google/protobuf/reflection_ops.h>
#include <google/protobuf/wire_format.h>
// @@protoc_insertion_point(includes)

namespace ice_sProtol {

namespace {

const ::google::protobuf::EnumDescriptor* MSGID_descriptor_ = NULL;

}  // namespace


void protobuf_AssignDesc_ice_5fsProtol_2eproto() {
  protobuf_AddDesc_ice_5fsProtol_2eproto();
  const ::google::protobuf::FileDescriptor* file =
    ::google::protobuf::DescriptorPool::generated_pool()->FindFileByName(
      "ice_sProtol.proto");
  GOOGLE_CHECK(file != NULL);
  MSGID_descriptor_ = file->enum_type(0);
}

namespace {

GOOGLE_PROTOBUF_DECLARE_ONCE(protobuf_AssignDescriptors_once_);
inline void protobuf_AssignDescriptorsOnce() {
  ::google::protobuf::GoogleOnceInit(&protobuf_AssignDescriptors_once_,
                 &protobuf_AssignDesc_ice_5fsProtol_2eproto);
}

void protobuf_RegisterTypes(const ::std::string&) {
  protobuf_AssignDescriptorsOnce();
}

}  // namespace

void protobuf_ShutdownFile_ice_5fsProtol_2eproto() {
}

void protobuf_AddDesc_ice_5fsProtol_2eproto() {
  static bool already_here = false;
  if (already_here) return;
  already_here = true;
  GOOGLE_PROTOBUF_VERIFY_VERSION;

  ::google::protobuf::DescriptorPool::InternalAddGeneratedFile(
    "\n\021ice_sProtol.proto\022\013ice_sProtol*o\n\005MSGI"
    "D\022\031\n\024CSID_C2L_LOGINSERVER\020\374\007\022\031\n\024CSID_L2C"
    "_LOGINSERVER\020\375\007\022\027\n\021CSID_C2S_PUB_BEAT\020\224\353\001"
    "\022\027\n\021CSID_S2C_PUB_BEAT\020\225\353\001", 145);
  ::google::protobuf::MessageFactory::InternalRegisterGeneratedFile(
    "ice_sProtol.proto", &protobuf_RegisterTypes);
  ::google::protobuf::internal::OnShutdown(&protobuf_ShutdownFile_ice_5fsProtol_2eproto);
}

// Force AddDescriptors() to be called at static initialization time.
struct StaticDescriptorInitializer_ice_5fsProtol_2eproto {
  StaticDescriptorInitializer_ice_5fsProtol_2eproto() {
    protobuf_AddDesc_ice_5fsProtol_2eproto();
  }
} static_descriptor_initializer_ice_5fsProtol_2eproto_;
const ::google::protobuf::EnumDescriptor* MSGID_descriptor() {
  protobuf_AssignDescriptorsOnce();
  return MSGID_descriptor_;
}
bool MSGID_IsValid(int value) {
  switch(value) {
    case 1020:
    case 1021:
    case 30100:
    case 30101:
      return true;
    default:
      return false;
  }
}


// @@protoc_insertion_point(namespace_scope)

}  // namespace ice_sProtol

// @@protoc_insertion_point(global_scope)
