//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Option: light framework (CF/Silverlight) enabled
    
// Generated from: protobuf/mail.proto
namespace com.kz.message.proto
{
  [global::ProtoBuf.ProtoContract(Name=@"MailListPro")]
  public partial class MailListPro : global::ProtoBuf.IExtensible
  {
    public MailListPro() {}
    
    private readonly global::System.Collections.Generic.List<com.kz.message.proto.MailPro> _mailList = new global::System.Collections.Generic.List<com.kz.message.proto.MailPro>();
    [global::ProtoBuf.ProtoMember(1, Name=@"mailList", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<com.kz.message.proto.MailPro> mailList
    {
      get { return _mailList; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::ProtoBuf.ProtoContract(Name=@"MailPro")]
  public partial class MailPro : global::ProtoBuf.IExtensible
  {
    public MailPro() {}
    
    private long _roleId;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"roleId", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public long roleId
    {
      get { return _roleId; }
      set { _roleId = value; }
    }
    private long _mailId;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"mailId", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public long mailId
    {
      get { return _mailId; }
      set { _mailId = value; }
    }
    private long _time;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"time", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public long time
    {
      get { return _time; }
      set { _time = value; }
    }
    private string _content = "";
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"content", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string content
    {
      get { return _content; }
      set { _content = value; }
    }
    private string _attach = "";
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"attach", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string attach
    {
      get { return _attach; }
      set { _attach = value; }
    }
    private string _sender = "";
    [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"sender", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string sender
    {
      get { return _sender; }
      set { _sender = value; }
    }
    private string _title = "";
    [global::ProtoBuf.ProtoMember(7, IsRequired = false, Name=@"title", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string title
    {
      get { return _title; }
      set { _title = value; }
    }
    private int _type;
    [global::ProtoBuf.ProtoMember(8, IsRequired = true, Name=@"type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int type
    {
      get { return _type; }
      set { _type = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}